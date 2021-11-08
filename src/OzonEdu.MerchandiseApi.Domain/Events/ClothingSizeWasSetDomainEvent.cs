using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class ClothingSizeWasSetDomainEvent : INotification
    {
        public int EmployeeId { get; }
        public ClothingSize Size { get; }

        public ClothingSizeWasSetDomainEvent(int employeeId, ClothingSize size)
        {
            EmployeeId = employeeId;
            Size = size;
        }
    }
}