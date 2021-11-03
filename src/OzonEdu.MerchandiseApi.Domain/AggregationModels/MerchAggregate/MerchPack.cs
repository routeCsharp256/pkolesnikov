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
                return;

            if (MerchPackType == MerchPackType.ConferenceListener
                || MerchPackType == MerchPackType.ConferenceListener
                || MerchPackType == MerchPackType.ConferenceSpeaker)
                InitiatingEventName = eventName;
        }
    }
}