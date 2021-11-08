using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Exceptions.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPack : Entity
    {
        private MerchPackStatus _status = MerchPackStatus.InWork;
        
        public MerchPackType Type { get; }

        public MerchPackStatus Status
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

        public MerchPack(int id, MerchPackType type, IEnumerable<Sku> skuCollection, MerchPackStatus status)
            : this(type, skuCollection, status)
        {
            Id = id;
        }
        
        public MerchPack(MerchPackType type, IEnumerable<Sku> skuCollection, MerchPackStatus status)
        {
            Type = type;
            SkuCollection = skuCollection;
            SetStatus(status);
        }

        public void SetInitiatingEventName(InitiatingEventName? eventName)
        {
            if (eventName is null)
                throw new ArgumentNullException(nameof(eventName));

            if (Type.Equals(MerchPackType.ConferenceListenerPack)
                || Type.Equals(MerchPackType.ConferenceListenerPack)
                || Type.Equals(MerchPackType.ConferenceSpeakerPack))
                InitiatingEventName = eventName;
        }
        
        public void SetStatus(MerchPackStatus newStatus)
        { 
            if (Status.Equals(MerchPackStatus.Done))
                throw new MerchDeliveryAlreadyDone($"The application (id={Id}) was completed");
            Status = newStatus; 
        }
    }
}