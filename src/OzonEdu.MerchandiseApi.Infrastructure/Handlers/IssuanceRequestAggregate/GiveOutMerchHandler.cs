using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.IssuanceRequestAggregate
{
    public class GiveOutMerchHandler : IRequestHandler<GiveOutMerchCommand>
    {
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GiveOutMerchHandler(IEmployeeRepository employeeRepository,
            IMerchPackRepository merchPackRepository)
        {
            _employeeRepository = employeeRepository;
            _merchPackRepository = merchPackRepository;
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

            var merchPack = employee
                .MerchPacks
                .FirstOrDefault(mp => mp.Equals(request.MerchPackTypeId));
            
            #region Если мерч даже не пробовали выдавать - создаём заявку
            if (merchPack is null)
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

                var merchPackForSave = new MerchPack(MerchPackType.WelcomePack, skuCollection, MerchPackStatus.InWork);
                merchPack = await _merchPackRepository.CreateAsync(merchPackForSave, token);
                if (merchPack is null)
                    throw new Exception("create merch pack error");
                
                employee.AddMerchPack(merchPack);
                await _employeeRepository.UpdateAsync(employee, token);
            }

            #endregion

            #region Если мерч уже выдавался - выход
            if (merchPack.Status == MerchPackStatus.Done)
                return Unit.Value;
            #endregion

            #region Проверка каждого SKU, что его можно выдать

            var canDelivery = true;
                
            #endregion

            #region Если невозможно выдать мерч, то выставляем соответствующий статус

            if (!canDelivery)
            {
                var newStatus = request.IsManual
                    ? MerchPackStatus.EmployeeCame
                    : MerchPackStatus.Notify;
                merchPack.SetStatus(newStatus);
                await _employeeRepository.UpdateAsync(employee, token);
                return Unit.Value;
            }

            #endregion

            #region Отправляем в Stock API запрос на резервирование
            #endregion
            
            merchPack.SetStatus(MerchPackStatus.Done);
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