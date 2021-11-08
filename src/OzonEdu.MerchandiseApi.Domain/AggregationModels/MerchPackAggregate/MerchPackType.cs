using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.Models;
using Enums = CSharpCourse.Core.Lib.Enums;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPackType : MerchTypeEnumeration
    {
        public static MerchPackType WelcomePack 
            = new(Enums.MerchType.WelcomePack, new [] { MerchType.Pen });

        public static MerchPackType ProbationPeriodEndingPack 
            = new(Enums.MerchType.ProbationPeriodEndingPack, new [] { MerchType.Socks });
        
        public static MerchPackType ConferenceListenerPack 
            = new(Enums.MerchType.ConferenceListenerPack, new [] { MerchType.Notepad });
        
        public static MerchPackType ConferenceSpeakerPack 
            = new(Enums.MerchType.ConferenceSpeakerPack, new [] { MerchType.TShirt });
        
        public static MerchPackType VeteranPack 
            = new(Enums.MerchType.VeteranPack, new []{ MerchType.Bag, MerchType.Sweatshirt });
        
        public MerchPackType(Enums.MerchType packType, IEnumerable<MerchType> merchTypes) 
            : base((int)packType, packType.ToString(), merchTypes)
        { }
    }
}