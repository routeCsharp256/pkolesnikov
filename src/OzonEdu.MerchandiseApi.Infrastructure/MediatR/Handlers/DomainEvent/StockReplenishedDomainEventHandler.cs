using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors;
using OzonEdu.MerchandiseApi.Infrastructure.Configuration;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.MessageBroker;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.DomainEvent
{
    public class StockReplenishedDomainEventHandler : INotificationHandler<StockReplenishedDomainEvent>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMediator _mediator;
        private readonly StockApiGrpc.StockApiGrpcClient _stockClient;
        private readonly ILogger<StockReplenishedDomainEventHandler> _logger;
        private readonly KafkaManager _kafka;

        public StockReplenishedDomainEventHandler(IEmployeeService employeeService,
            IMediator mediator,
            StockApiGrpc.StockApiGrpcClient stockClient,
            ILogger<StockReplenishedDomainEventHandler> logger,
            KafkaManager kafka)
        {
            _employeeService = employeeService;
            _mediator = mediator;
            _stockClient = stockClient;
            _logger = logger;
            _kafka = kafka;
        }
        
        public async Task Handle(StockReplenishedDomainEvent notification, CancellationToken token)
        {
            var skuCollection = notification
                .Items
                .Select(it => it.Sku)
                .ToArray();

            var manual = new ManualAppealProcessor(skuCollection);
            manual.Do();
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

            var deliveries = employeesForNotify
                .SelectMany(e => e.MerchDeliveries,
                    (e, md) => new
                    {
                        Employee = e, 
                        MerchDelivery = md
                    });

            foreach (var delivery in deliveries)
            {
                var employee = delivery.Employee;
                if (employee.EmailAddress is null)
                {
                    _logger.LogWarning("The email of employee {name} is not specified", 
                        employee.Name.Value);
                    continue;
                }

                var request = new SkusRequest();
                request.Skus.AddRange(delivery
                    .MerchDelivery
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

                var topic = _kafka
                    .Configuration
                    .EmployeeNotificationEventTopic;
                
                var key = delivery
                    .MerchDelivery
                    .Id
                    .ToString();
                
                var notificationEvent = new NotificationEvent
                {
                    EmployeeEmail = employee.EmailAddress.Value,
                    EmployeeName = employee.Name.Value,
                    EventType = EmployeeEventType.MerchDelivery,
                    Payload = new
                    {
                        MerchType = delivery
                            .MerchDelivery
                            .MerchPackType
                            .Id
                    }
                };
                
                await _kafka.ProcessAsync(topic, key, notificationEvent, token);
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