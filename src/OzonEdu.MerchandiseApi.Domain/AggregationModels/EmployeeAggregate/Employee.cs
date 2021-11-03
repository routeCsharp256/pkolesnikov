using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public EmployeeId EmployeeId { get; }
        
        public Name Name { get; }
        
        public ClothingSize ClothingSize { get; }
        
        public EmailAddress? EmailAddress { get; set; }

        public Employee(EmployeeId id, Name name, ClothingSize size, EmailAddress? email)
        {
            EmployeeId = id;
            Name = name;
            ClothingSize = size;
            SetEmailAddress(email);
        }

        public void SetEmailAddress(EmailAddress? email)
        {
            if (email is null)
                return;
            
            EmailAddress = email;
        }
    }
}