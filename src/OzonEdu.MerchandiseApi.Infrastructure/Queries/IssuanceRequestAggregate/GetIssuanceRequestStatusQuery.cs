using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.Queries.IssuanceRequestAggregate
{
    public class GetIssuanceRequestStatusQuery : IRequest<int>
    {
        public long EmployeeId { get; set; }
        public int MerchPackId { get; set; }
    }
}