using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.Queries.IssuanceRequestAggregate
{
    public class GetMerchDeliveryStatusQuery : IRequest<MerchDeliveryStatus?>
    {
        public long EmployeeId { get; set; }
        public int MerchPackTypeId { get; set; }
    }
}