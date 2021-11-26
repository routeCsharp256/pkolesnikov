using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;
using OzonEdu.MerchandiseApi.Domain.Services.Exceptions;
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
            var employee = await _employeeService.FindAsync(request.EmployeeId, token);
            if (employee is null)
                throw new NotExistsException($"Employee with id={request.EmployeeId} does not exists");

            var merchPackType = await _merchService.FindMerchPackType(request.MerchPackTypeId, token);
            if (merchPackType is null)
            {
                throw new NotExistsException(
                    $"Merch pack type with id={request.MerchPackTypeId} does not exists");
            }

            var merchDelivery = employee
                                    .MerchDeliveries
                                    .FirstOrDefault(d => d.MerchPackType.Id == merchPackType.Id);

            if (merchDelivery is null)
            {
                merchDelivery = await _merchService.CreateMerchDeliveryAsync((MerchType)merchPackType.Id, 
                    employee.ClothingSize, 
                    token);
                
                await _employeeService.AddMerchDelivery(employee.Id, merchDelivery.Id, token);
            }
            
            if (merchDelivery.Status.Equals(MerchDeliveryStatus.Done))
                return Unit.Value;

            var newStatus = MerchDeliveryStatus.Done;

            if (await CanDelivery(merchDelivery.SkuCollection))
            {
                // TODO Отправка в Stock API запрос на резервирование
            }
            else
            {
                // TODO Здесь будет отправка сообщения HR, что закончился мерч с таким-то SKU (для автоматической выдачи)
                newStatus = request.IsManual
                    ? MerchDeliveryStatus.EmployeeCame
                    : MerchDeliveryStatus.Notify;
            }
            
            merchDelivery.SetStatus(newStatus);
            await _merchService.UpdateAsync(merchDelivery, token);
            return Unit.Value;
        }

        private async Task<bool> CanDelivery(ICollection<Sku> skuCollection)
        {
            // TODO Проверка каждого SKU, что его можно выдать
            return await Task.FromResult(true);
        }
    }
}