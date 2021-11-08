using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseApi.Domain.Events;

namespace OzonEdu.MerchandiseApi.Infrastructure.Handlers.DomainEvent
{
    public class HiringDomainEventHandler : INotificationHandler<HiringDomainEvent>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMerchPackRepository _merchPackRepository;

        public HiringDomainEventHandler(IEmployeeRepository employeeRepository, 
            IMerchPackRepository merchPackRepository)
        {
            _employeeRepository = employeeRepository;
            _merchPackRepository = merchPackRepository;
        }
        
        /// <summary>
        ///     Записываем сотрудника в БД merch-сервиса. Считаем, что стартовый набор можно выдать (нет одежды).
        /// </summary>
        /// <param name="notification"> Уведомление о найме нового сотрудника на работу. </param>
        /// <param name="token"> Токен отмены. </param>
        public async Task Handle(HiringDomainEvent notification, CancellationToken token)
        {
            var employee = await _employeeRepository.CreateAsync(notification.Employee, token);
            if (employee is null)
                throw new Exception("employee wasn't created");

            var merchItemIds = MerchPackType
                .WelcomePack
                .MerchTypes
                .Select(mt => mt.Id)
                .ToArray();
            
            // Следует запрос к Stock API для получения SKU и количества по идентификаторам.
            var skuCollection = new List<Sku>();
            
            // Следует проверка, что каждый SKU можно выдать.
            var canDelivery = true;
            
            if (!canDelivery)
            {
                var merchPackForSave = new MerchPack(MerchPackType.WelcomePack, skuCollection, MerchPackStatus.Notify);
                var merchPack = await _merchPackRepository.CreateAsync(merchPackForSave, token);
                if (merchPack is null)
                    throw new Exception("create merch pack error");
                
                employee.AddMerchPack(merchPack);
                return;
            }

            // Зарезервировать мерч в Stock API
            
            
        }
    }
}