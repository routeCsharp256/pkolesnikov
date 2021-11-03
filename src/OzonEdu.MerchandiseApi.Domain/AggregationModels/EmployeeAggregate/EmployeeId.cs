using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class EmployeeId : ValueObject
    {
        public long Value { get; }
        
        public EmployeeId(long sku)
        {
            Value = sku;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}