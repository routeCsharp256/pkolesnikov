using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPackType : MerchTypeEnumeration
    {
        public static MerchPackType Welcome = new(1, 
            nameof(Welcome), 
            GetAll<MerchType>().Where(mt => mt.Equals(MerchType.Pen)).ToArray());
        
        public static MerchPackType Starter = 
            new(2, 
                nameof(Starter), 
                GetAll<MerchType>().Where(mt => mt.Equals(MerchType.Socks)).ToArray());
        
        public static MerchPackType ConferenceListener = 
            new(3, 
                nameof(ConferenceListener), 
                GetAll<MerchType>().Where(mt => mt.Equals(MerchType.Notepad)).ToArray());
        
        public static MerchPackType ConferenceSpeaker = 
            new(4, 
                nameof(ConferenceSpeaker), 
                GetAll<MerchType>()
                    .Where(mt => mt.Equals(MerchType.TShirt))
                    .ToArray());
        
        public static MerchPackType Veteran = 
            new(5, 
                nameof(Veteran), 
                GetAll<MerchType>()
                    .Where(mt => mt.Equals(MerchType.Bag) ||  mt.Equals(MerchType.Sweatshirt))
                    .ToArray());
        
        public MerchPackType(int id, string name, IEnumerable<MerchType> merchTypes) : base(id, name, merchTypes)
        { }
    }
}