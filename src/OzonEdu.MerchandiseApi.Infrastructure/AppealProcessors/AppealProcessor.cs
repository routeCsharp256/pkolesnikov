using System.Collections.Generic;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors
{
    internal abstract class AppealProcessor
    {
        protected readonly IEnumerable<long> _skuCollection;

        protected abstract MerchDeliveryStatus MerchDeliveryStatus { get; set; }

        internal AppealProcessor(IEnumerable<long> skuCollection)
        {
            _skuCollection = skuCollection;
        }

        internal abstract Task Do();
    }
}