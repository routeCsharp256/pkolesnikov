using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.DomainEvent
{
    public class StockReplenishedDomainEventHandler : INotificationHandler<StockReplenishedDomainEvent>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMediator _mediator;
        private readonly StockApiGrpc.StockApiGrpcClient _stockClient;
        
        public StockReplenishedDomainEventHandler(IEmployeeService employeeService,
            IMediator mediator,
            StockApiGrpc.StockApiGrpcClient stockClient)
        {
            _employeeService = employeeService;
            _mediator = mediator;
            _stockClient = stockClient;
        }
        
        public async Task Handle(StockReplenishedDomainEvent notification, CancellationToken token)
        {
            var skuCollection = notification
                .Items
                .Select(it => it.Sku)
                .ToArray();
            
            await NotifyByEmail(skuCollection, token);
            await TryToGiveUp(skuCollection, token);
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

            var deliveries = employeesForNotify?
                .SelectMany(e => e.MerchDeliveries,
                    (employee, md) => new { Employee = employee, SkuCollection = md.SkuCollection });

            if (deliveries is null)
                return;

            foreach (var delivery in deliveries)
            {
                
                // TODO Проверить, что все товары есть на складе
                var request = new SkusRequest();
                request.Skus.AddRange(delivery
                    .SkuCollection
                    .Select(s => s.Value)
                    .ToArray());
                
                var response = await _stockClient
                    .GetStockItemsAvailabilityAsync(request, cancellationToken: token);
                
                var isReadyToGiveOut = response
                    .Items
                    .All(i => i.Quantity > 0);

                if (!isReadyToGiveOut)
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