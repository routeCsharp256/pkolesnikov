using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; }
        
        public EmailAddress(string name)
        {
            Value = name;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}