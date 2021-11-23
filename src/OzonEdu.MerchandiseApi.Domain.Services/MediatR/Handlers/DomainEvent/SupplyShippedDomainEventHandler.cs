using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Commands;

namespace OzonEdu.MerchandiseApi.Domain.Services.MediatR.Handlers.DomainEvent
{
    public class SupplyShippedDomainEventHandler : INotificationHandler<SupplyShippedDomainEvent>
    {
        private readonly IMerchService _merchService;
        private readonly IEmployeeService _employeeService;
        private readonly IMediator _mediator;

        public SupplyShippedDomainEventHandler(IMerchService merchService,
            IEmployeeService employeeService,
            IMediator mediator)
        {
            _merchService = merchService;
            _employeeService = employeeService;
            _mediator = mediator;
        }
        
        public async Task Handle(SupplyShippedDomainEvent notification, CancellationToken token)
        {
            var suppliedSkuCollection = notification
                .SupplyShippedEvent
                .Items
                .Select(i => i.SkuId)
                .ToArray();
            
            await NotifyByEmail(suppliedSkuCollection, token);
            await TryToGiveUp(suppliedSkuCollection, token);
        }

        private async Task NotifyByEmail(IEnumerable<long> suppliedSkuCollection, CancellationToken token)
        {
            var employeeCameStatus = MerchDeliveryStatus
                .GetAll<MerchDeliveryStatus>()
                .FirstOrDefault(s => s.Equals(MerchDeliveryStatus.EmployeeCame));

            if (employeeCameStatus is null)
                throw new Exception("\"Employee came\" status not found");

            var employeesForNotify = await _employeeService
                .GetAsync(employeeCameStatus, suppliedSkuCollection, token);

            foreach (var employee in employeesForNotify)
            {
                var skuCollection = employee
                    .MerchDeliveries
                    .First(md => md.Status.Equals(MerchDeliveryStatus.EmployeeCame));
                
                // TODO Проверить, что все товары есть на складе
                var isAllRight = true;

                if (!isAllRight)
                    continue;
                
                // TODO Уведомить сотрудника по email, что он может прийти за мерчем
                var email = employee.EmailAddress;

                return;
            }
        }

        private async Task TryToGiveUp(IEnumerable<long> suppliedSkuCollection, CancellationToken token)
        {
            var notifyStatus = MerchDeliveryStatus
                .GetAll<MerchDeliveryStatus>()
                .FirstOrDefault(s => s.Equals(MerchDeliveryStatus.Notify));

            if (notifyStatus is null)
                throw new Exception("\"Notify\" status not found");

            var employeesForNotify = await _employeeService
                .GetAsync(MerchDeliveryStatus.Notify, suppliedSkuCollection, token);

            foreach (var employee in employeesForNotify)
            {
                var delivery = employee
                    .MerchDeliveries
                    .First(md => md.Status.Equals(MerchDeliveryStatus.Notify));
                
                var command = new GiveOutMerchCommand
                {
                    EmployeeId = employee.Id,
                    MerchPackTypeId = delivery.MerchPackType.Id,
                    IsManual = false
                };
                await _mediator.Send(command, token);
            }
        }
    }
}