using System;
using System.Collections.Generic;
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
        
        public IEnumerable<Sku> SkuCollection { get; }

        public InitiatingEventName? InitiatingEventName { get; private set; }
        
        public StatusChangeDate StatusChangeDate { get; private set; } = new(DateTime.UtcNow);

        // public MerchPack(MerchPackId id, MerchPackType type, MerchPackName name, InitiatingEventName? eventName)
        //     : this(id, type)
        // {
        //     SetInitiatingEventName(eventName);
        // }

        public MerchDelivery(int id, MerchPackType type, IEnumerable<Sku> skuCollection, MerchDeliveryStatus status)
            : this(type, skuCollection, status)
        {
            Id = id;
        }
        
        public MerchDelivery(MerchPackType merchPackType, IEnumerable<Sku> skuCollection, MerchDeliveryStatus status)
        {
            MerchPackType = merchPackType;
            SkuCollection = skuCollection;
            SetStatus(status);
        }

        public void SetInitiatingEventName(InitiatingEventName? eventName)
        {
            if (eventName is null)
                throw new ArgumentNullException(nameof(eventName));

            if (MerchPackType.Equals(MerchPackType.ConferenceListenerPack)
                || MerchPackType.Equals(MerchPackType.ConferenceListenerPack)
                || MerchPackType.Equals(MerchPackType.ConferenceSpeakerPack))
                InitiatingEventName = eventName;
        }
        
        public void SetStatus(MerchDeliveryStatus newStatus)
        { 
            if (Status.Equals(MerchDeliveryStatus.Done))
                throw new MerchDeliveryAlreadyDone($"The application (id={Id}) was completed");
            Status = newStatus; 
        }
    }
}