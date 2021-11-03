using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class MerchItemId : ValueObject
    {
        public long Value { get; }

        public MerchItemId(long id)
        {
            Value = id;
        }
        
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}