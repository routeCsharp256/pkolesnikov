using System;
using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.Exceptions;
using OzonEdu.MerchandiseApi.Domain.Exceptions.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class MerchDelivery : Entity
    {
        private MerchDeliveryStatus _status = MerchDeliveryStatus.InWork;
        
        public MerchPackType MerchPackType { get; }

        public MerchDeliveryStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                StatusChangeDate = new StatusChangeDate(DateTime.UtcNow);
            }
        }

        public ICollection<Sku> SkuCollection { get; } = new List<Sku>();
        
        public StatusChangeDate StatusChangeDate { get; private set; } = new(DateTime.UtcNow);

        public MerchDelivery(int id, MerchPackType type, IEnumerable<Sku> skuCollection, MerchDeliveryStatus status)
            : this(type, skuCollection, status)
        {
            Id = id;
        }
        
        public MerchDelivery(MerchPackType merchPackType, IEnumerable<Sku> skuCollection, MerchDeliveryStatus status)
        : this(merchPackType, status)
        {
            SetSkuCollection(skuCollection);
        }
        
        public MerchDelivery(MerchPackType merchPackType, MerchDeliveryStatus status)
        {
            MerchPackType = merchPackType;
            SetStatus(status);
        }
        
        public void SetStatus(MerchDeliveryStatus newStatus)
        { 
            if (Status.Equals(MerchDeliveryStatus.Done))
                throw new MerchDeliveryAlreadyDone($"The application (id={Id}) was completed");
            Status = newStatus; 
        }

        public void SetSkuCollection(IEnumerable<Sku> skuCollection)
        {
            var hasElements = false;
            
            foreach (var sku in skuCollection)
            {
                if (sku.Value < 0)
                    throw new NegativeValueException("sku value is less zero");
                SkuCollection
                    .Add(sku);
                hasElements = true;
            }

            if (!hasElements)
                throw new EmptyCollectionException("Sku collection is empty");
        }
    }
}