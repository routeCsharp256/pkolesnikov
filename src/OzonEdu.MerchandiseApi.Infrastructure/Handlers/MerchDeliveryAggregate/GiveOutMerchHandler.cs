using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.MerchDeliveryAggregate
{
    public class GiveOutMerchHandler : IRequestHandler<GiveOutMerchCommand>
    {
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GiveOutMerchHandler(IEmployeeRepository employeeRepository,
            IMerchDeliveryRepository merchDeliveryRepository)
        {
            _employeeRepository = employeeRepository;
            _merchDeliveryRepository = merchDeliveryRepository;
        }
        
        public async Task<Unit> Handle(GiveOutMerchCommand request, CancellationToken token)
        {
            var merchPackTypeId = request.MerchPackTypeId;
            if (IsExistsMerchPackType(merchPackTypeId))
                throw new Exception("Id of merch pack type isn't correct");
            
            var employeeId = request.EmployeeId;
            var employee = await _employeeRepository.FindByIdAsync(employeeId, token);
            if (employee is null)
            {
                throw new Exception("employee not found");
            }

            var merchDelivery = employee
                .MerchDeliveries
                .FirstOrDefault(mp => mp.Equals(request.MerchPackTypeId));
            
            #region Если мерч даже не пробовали выдавать - создаём заявку
            if (merchDelivery is null)
            {
                var merchPackType = MerchPackType
                    .GetAll<MerchPackType>()
                    .First(t => t.Id == merchPackTypeId);

                #region По идентификатору и размеру можно из Stock API получить SKU мерча, входящего в набор

                var merchItemIds = merchPackType
                    .MerchTypes
                    .Select(mt => mt.Id)
                    .ToArray();
                var size = employee.ClothingSize?.Id;

                var skuCollection = new List<Sku>();
                
                #endregion

                var merchPackForSave = new MerchDelivery(MerchPackType.WelcomePack, skuCollection, MerchDeliveryStatus.InWork);
                merchDelivery = await _merchDeliveryRepository.CreateAsync(merchPackForSave, token);
                if (merchDelivery is null)
                    throw new Exception("create merch pack error");
                
                employee.AddMerchDelivery(merchDelivery);
                await _employeeRepository.UpdateAsync(employee, token);
            }

            #endregion

            #region Если мерч уже выдавался - выход
            if (merchDelivery.Status == MerchDeliveryStatus.Done)
                return Unit.Value;
            #endregion

            #region Проверка каждого SKU, что его можно выдать

            var canDelivery = true;
                
            #endregion

            #region Если невозможно выдать мерч, то выставляем соответствующий статус

            if (!canDelivery)
            {

                #region Здесь будет отправка сообщения HR, что закончился мерч с таким-то SKU (для автоматической выдачи)

                

                #endregion
                
                var newStatus = request.IsManual
                    ? MerchDeliveryStatus.EmployeeCame
                    : MerchDeliveryStatus.Notify;
                merchDelivery.SetStatus(newStatus);
                await _employeeRepository.UpdateAsync(employee, token);
                return Unit.Value;
            }

            #endregion

            #region Отправляем в Stock API запрос на резервирование
            #endregion
            
            merchDelivery.SetStatus(MerchDeliveryStatus.Done);
            await _employeeRepository.UpdateAsync(employee, token);
            return Unit.Value;
        }

        private static bool IsExistsMerchPackType(int id)
        {
            return MerchPackType
                .GetAll<MerchPackType>()
                .Select(t => t.Id)
                .Contains(id);
        }
    }
}