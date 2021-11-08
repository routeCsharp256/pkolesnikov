using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPackType : MerchTypeEnumeration
    {
        public static MerchPackType WelcomePack = new(10, 
            nameof(WelcomePack), 
            GetAll<MerchType>().Where(mt => mt.Equals(MerchType.Pen)).ToArray());
        
        public static MerchPackType ProbationPeriodEndingPack = 
            new(40, 
                nameof(ProbationPeriodEndingPack), 
                GetAll<MerchType>().Where(mt => mt.Equals(MerchType.Socks)).ToArray());
        
        public static MerchPackType ConferenceListenerPack = 
            new(20, 
                nameof(ConferenceListenerPack), 
                GetAll<MerchType>().Where(mt => mt.Equals(MerchType.Notepad)).ToArray());
        
        public static MerchPackType ConferenceSpeakerPack = 
            new(30, 
                nameof(ConferenceSpeakerPack), 
                GetAll<MerchType>()
                    .Where(mt => mt.Equals(MerchType.TShirt))
                    .ToArray());
        
        public static MerchPackType VeteranPack = 
            new(50, 
                nameof(VeteranPack), 
                GetAll<MerchType>()
                    .Where(mt => mt.Equals(MerchType.Bag) ||  mt.Equals(MerchType.Sweatshirt))
                    .ToArray());
        
        public MerchPackType(int id, string name, IEnumerable<MerchType> merchTypes) : base(id, name, merchTypes)
        { }
    }
}