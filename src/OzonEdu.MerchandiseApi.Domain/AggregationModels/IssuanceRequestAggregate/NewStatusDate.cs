using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class NewStatusDate : ValueObject
    {
        public DateTime Value { get; }

        public NewStatusDate(DateTime date)
        {
            Value = date;
        }
        
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}