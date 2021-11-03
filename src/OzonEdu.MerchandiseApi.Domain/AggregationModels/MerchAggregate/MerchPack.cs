using System;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class MerchPack : Entity
    {
        public MerchPackId MerchPackId { get; }
        
        public MerchPackType MerchPackType { get; }
        
        public InitiatingEventName? InitiatingEventName { get; private set; }

        public MerchPack(MerchPackId id, MerchPackType packType, InitiatingEventName? eventName)
        {
            MerchPackId = id;
            MerchPackType = packType;
            SetInitiatingEventName(eventName);
        }
        
        public void SetInitiatingEventName(InitiatingEventName? eventName)
        {
            if (eventName is null)
                throw new ArgumentNullException(nameof(eventName));

            if (MerchPackType.Equals(MerchPackType.ConferenceListener)
                || MerchPackType.Equals(MerchPackType.ConferenceListener)
                || MerchPackType.Equals(MerchPackType.ConferenceSpeaker))
                InitiatingEventName = eventName;
        }
    }
}