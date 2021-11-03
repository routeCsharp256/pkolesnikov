using System;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class MerchPack : Entity
    {
        public MerchPackId MerchPackId { get; }
        
        public MerchType MerchType { get; }

        public MerchPack(MerchPackId id, MerchType type)
        {
            MerchPackId = id;
            MerchType = type;
        }
    }
}