using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class MerchPackType : MerchItemIdEnumeration
    {
        public static MerchPackType Welcome = 
            new(1, nameof(Welcome), new MerchItemId[] { new (1) });
        
        public static MerchPackType Starter = 
            new(2, nameof(Starter), new MerchItemId[] { new (1), new (2) });
        
        public static MerchPackType ConferenceListener = 
            new(3, nameof(ConferenceListener), new MerchItemId[] { new (1), new (2), new(3) });
        
        public static MerchPackType ConferenceSpeaker = 
            new(4, nameof(ConferenceSpeaker), new MerchItemId[] { new (1), new (2), new(3), new(4) });
        
        public static MerchPackType Veteran = 
            new(5, nameof(Veteran), new MerchItemId[] { new (1), new (2), new(3), new(4), new(5) });
        
        public MerchPackType(int id, string name, IEnumerable<MerchItemId> items) : base(id, name, items)
        { }
    }
}