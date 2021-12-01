using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Tracers;
using MerchType = CSharpCourse.Core.Lib.Enums.MerchType;

namespace OzonEdu.MerchandiseApi.Infrastructure.Services.Implementation
{
    public class MerchService : IMerchService
    {
        private const string ServiceName = nameof(MerchService);
        
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;
        private readonly CustomTracer _tracer;

        public MerchService(IMerchDeliveryRepository merchDeliveryRepository, CustomTracer tracer)
        {
            _merchDeliveryRepository = merchDeliveryRepository;
            _tracer = tracer;
        }
        
        public async Task<MerchDelivery> CreateMerchDeliveryAsync(MerchType merchType,
            ClothingSize? size,
            CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(CreateMerchDeliveryAsync));
            
            var merchPackType = MerchPackType
                .GetAll<MerchPackType>()
                .FirstOrDefault(t => t.Id == (int)merchType);
                
            if (merchPackType is null)
                throw new Exception("Unknown merch pack type");

            if (size is null)
                throw new Exception("Employee's clothing size is not specified");

            // TODO Здесь будем получать список SKU из stock-сервиса по типу и размеру
            var skuList = new List<Sku>();

            var deliveryData = new MerchDelivery(merchPackType,
                skuList,
                MerchDeliveryStatus.InWork);
            
            var merchDelivery = await _merchDeliveryRepository.CreateAsync(deliveryData, token);
            
            if (merchDelivery is null)
                throw new Exception("Merch delivery wasn't created");
            
            return merchDelivery;
        }

        public async Task<MerchDelivery?> UpdateAsync(MerchDelivery delivery, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(UpdateAsync));
            return await _merchDeliveryRepository.UpdateAsync(delivery, token);
        }

        public async Task<MerchDeliveryStatus?> FindStatus(int employeeId, int merchPackTypeId, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(FindStatus));
            return await _merchDeliveryRepository.FindStatus(employeeId, merchPackTypeId, token);
        }

        public async Task<MerchPackType?> FindMerchPackType(int typeId, CancellationToken token)
        {
            using var span = _tracer.GetSpan(ServiceName, nameof(FindMerchPackType));
            return await _merchDeliveryRepository.FindMerchPackType(typeId, token);
        }
    }
}