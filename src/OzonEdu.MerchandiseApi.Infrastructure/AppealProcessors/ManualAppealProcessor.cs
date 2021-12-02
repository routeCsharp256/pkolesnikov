using System.Collections.Generic;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors
{
    internal class ManualAppealProcessor : AppealProcessor
    {
        protected override MerchDeliveryStatus MerchDeliveryStatus { get; set; } = MerchDeliveryStatus.EmployeeCame;

        internal ManualAppealProcessor(IEnumerable<long> skuCollection) : base(skuCollection)
        { }

        internal override Task Do()
        {
            throw new System.NotImplementedException();
        }
    }
}