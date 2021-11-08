using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.DomainEvent
{
    public class SupplyShippedDomainEventHandler : INotificationHandler<SupplyShippedDomainEvent>
    {
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMediator _mediator;

        public SupplyShippedDomainEventHandler(IMerchDeliveryRepository merchDeliveryRepository,
            IEmployeeRepository employeeRepository,
            IMediator mediator)
        {
            _merchDeliveryRepository = merchDeliveryRepository;
            _employeeRepository = employeeRepository;
            _mediator = mediator;
        }
        
        public async Task Handle(SupplyShippedDomainEvent notification, CancellationToken token)
        {
            var deliveries = await _merchDeliveryRepository.GetAll(token);

            var deliveriesForNotify = deliveries
                .Where(d => d.Status.Equals(MerchDeliveryStatus.EmployeeCame) 
                            && d.SkuCollection.Any(s => notification
                                .SupplyShippedEvent
                                .Items
                                .Select(i => i.SkuId)
                                .Any(ns => ns == s.Value)));

            foreach (var delivery in deliveriesForNotify)
            {
                var skuCollection = delivery.SkuCollection;
                
                #region Проверить, что все товары есть на складе

                var isAllRight = true;
                
                #endregion

                if (!isAllRight)
                    continue;

                #region Уведомить сотрудника по email, что он может прийти за мерчем

                

                #endregion
                
                return;
            }

            var deliveriesForGiveUp = deliveries
                .Where(d => d.Status.Equals(MerchDeliveryStatus.Notify) 
                            && d.SkuCollection.Any(s => notification
                                .SupplyShippedEvent
                                .Items
                                .Select(i => i.SkuId)
                                .Any(ns => ns == s.Value)));

            foreach (var delivery in deliveriesForGiveUp)
            {
                var employee = await _employeeRepository.FindByDeliveryId(delivery.Id, token);
                if (employee is null)
                    continue;
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