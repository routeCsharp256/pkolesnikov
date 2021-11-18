using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Name : ValueObject
    {
        public string Value { get; }
        
        public Name(string name)
        {
            Value = name;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}