using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Services.MediatR.Queries.IssuanceRequestAggregate
{
    public class GetMerchDeliveryStatusQuery : IRequest<MerchDeliveryStatus?>
    {
        public int EmployeeId { get; set; }
        public int MerchPackTypeId { get; set; }
    }
}