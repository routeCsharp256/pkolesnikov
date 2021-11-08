using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class HiringDomainEvent : INotification
    {
        public Employee Employee { get; }

        public HiringDomainEvent(Employee employee)
        {
            Employee = employee;
        }
    }
}