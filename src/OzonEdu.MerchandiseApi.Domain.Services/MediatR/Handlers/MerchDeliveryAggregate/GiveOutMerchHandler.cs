using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Commands;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchandiseApi.Domain.Services.MediatR.Handlers.MerchDeliveryAggregate
{
    public class GiveOutMerchHandler : IRequestHandler<GiveOutMerchCommand>
    {
        private readonly IMerchService _merchService;
        private readonly IEmployeeService _employeeService;

        public GiveOutMerchHandler(IMerchService merchService,
            IEmployeeService employeeService)
        {
            _merchService = merchService;
            _employeeService = employeeService;
        }
        
        public async Task<Unit> Handle(GiveOutMerchCommand request, CancellationToken token)
        {
            var merchPackTypeId = request.MerchPackTypeId;
            if (IsExistsMerchPackType(merchPackTypeId))
                throw new Exception("Id of merch pack type isn't correct");

            var employee = await _employeeService.FindAsync(request.EmployeeId, token);
            
            var merchDelivery = employee
                                    .MerchDeliveries
                                    .FirstOrDefault(d => d.MerchPackType.Id == merchPackTypeId);

            if (merchDelivery is null)
            {
                merchDelivery = await _merchService.CreateMerchDeliveryAsync((MerchType)merchPackTypeId, 
                    employee.ClothingSize, 
                    token);
                
                employee.AddMerchDelivery(merchDelivery);
                await _employeeService.UpdateAsync(employee, token);
            }
            
            if (merchDelivery.Status.Equals(MerchDeliveryStatus.Done))
                return Unit.Value;

            // TODO Проверка каждого SKU, что его можно выдать
            var canDelivery = true;

            if (!canDelivery)
            {
                // TODO Здесь будет отправка сообщения HR, что закончился мерч с таким-то SKU (для автоматической выдачи)
                
                var newStatus = request.IsManual
                    ? MerchDeliveryStatus.EmployeeCame
                    : MerchDeliveryStatus.Notify;
                merchDelivery.SetStatus(newStatus);
                await _employeeService.UpdateAsync(employee, token);
                return Unit.Value;
            }

            // TODO Отправка в Stock API запрос на резервирование

            merchDelivery.SetStatus(MerchDeliveryStatus.Done);
            await _employeeService.UpdateAsync(employee, token);
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