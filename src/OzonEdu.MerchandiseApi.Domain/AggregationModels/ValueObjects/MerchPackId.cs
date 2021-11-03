using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects
{
    public class MerchPackId : ValueObject
    {
        public int Value { get; }

        public MerchPackId(int id)
        {
            Value = id;
        }
        
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}