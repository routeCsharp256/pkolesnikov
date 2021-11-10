using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.Commands;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.DomainEvent
{
    public class EmployeeNotificationDomainEventHandler : INotificationHandler<EmployeeNotificationDomainEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;
        private readonly IMediator _mediator;

        public EmployeeNotificationDomainEventHandler(IEmployeeRepository employeeRepository, 
            IMerchDeliveryRepository merchDeliveryRepository,
            IMediator mediator)
        {
            _employeeRepository = employeeRepository;
            _merchDeliveryRepository = merchDeliveryRepository;
            _mediator = mediator;
        }
        
        /// <summary>
        ///     Записываем сотрудника в БД merch-сервиса. Считаем, что стартовый набор можно выдать (нет одежды).
        /// </summary>
        /// <param name="notification"> Уведомление о найме нового сотрудника на работу. </param>
        /// <param name="token"> Токен отмены. </param>
        public async Task Handle(EmployeeNotificationDomainEvent notification, CancellationToken token)
        {
            var notificationEvent = notification.NotificationEvent;
            var eventPayload = notificationEvent.Payload;
            if (eventPayload is not MerchDeliveryEventPayload merchDeliveryEventPayload)
                throw new Exception("Notification event payload isn't merch delivery");

            var merchType = merchDeliveryEventPayload.MerchType;
            if (!isTypeForReaction(merchType))
                throw new Exception("Notification event without reaction");

            var employee = await _employeeRepository.FindByEmailAsync(notificationEvent.EmployeeEmail, token);

            #region Если пользователь не найден по почте, то создаём его

            if (employee is null)
            {
                var employeeData = new Employee(
                    new Name(notificationEvent.EmployeeName),
                    new EmailAddress(notificationEvent.EmployeeEmail));
                
                employee = await _employeeRepository.CreateAsync(employeeData, token);
                if (employee is null)
                    throw new Exception("employee wasn't created");
            }

            #endregion

            var merchDelivery = employee
                .MerchDeliveries
                .FirstOrDefault(d => d.MerchPackType.Id == (int)merchType);

            #region Если выдача мерча не осуществлялась, то создаём её для дальнейших действий

            if (merchDelivery is null)
            {
                var merchPackType = MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(t => t.Id == (int)merchType);
                
                if (merchPackType is null)
                    throw new Exception("Unknown merch pack type");

                #region Получаем список SKU по типу мерча и размеру

                var skuList = new List<Sku>();

                #endregion
                
                var deliveryData = new MerchDelivery(merchPackType,
                    skuList,
                    MerchDeliveryStatus.InWork);
                merchDelivery = await _merchDeliveryRepository.CreateAsync(deliveryData, token);
                if (merchDelivery is null)
                    throw new Exception("Merch delivery wasn't created");
            }

            #endregion

            var command = new GiveOutMerchCommand
            {
                EmployeeId = employee.Id,
                MerchPackTypeId = merchDelivery.MerchPackType.Id,
                IsManual = false
            };
            await _mediator.Send(command, token);
        }

        private static bool isTypeForReaction(MerchType merchType)
        {
            return merchType is MerchType.WelcomePack 
                or MerchType.ConferenceListenerPack 
                or MerchType.ConferenceSpeakerPack;
        }
    }
}