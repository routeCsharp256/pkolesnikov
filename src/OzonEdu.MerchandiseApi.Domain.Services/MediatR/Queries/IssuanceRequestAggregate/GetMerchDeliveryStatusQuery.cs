using MediatR;

namespace OzonEdu.MerchandiseApi.Domain.Services.MediatR.Queries.IssuanceRequestAggregate
{
    public class GetMerchDeliveryStatusQuery : IRequest<string>
    {
        public int EmployeeId { get; set; }
        public int MerchPackTypeId { get; set; }
    }
}