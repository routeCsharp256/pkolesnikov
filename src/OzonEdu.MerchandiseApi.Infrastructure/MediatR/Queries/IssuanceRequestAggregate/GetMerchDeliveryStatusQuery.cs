using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Queries.IssuanceRequestAggregate
{
    public class GetMerchDeliveryStatusQuery : IRequest<string>
    {
        public int EmployeeId { get; set; }
        public int MerchPackTypeId { get; set; }
    }
}