using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class IssueDate : ValueObject
    {
        public DateTime Value { get; }

        public IssueDate(DateTime date)
        {
            Value = date;
        }
        
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}