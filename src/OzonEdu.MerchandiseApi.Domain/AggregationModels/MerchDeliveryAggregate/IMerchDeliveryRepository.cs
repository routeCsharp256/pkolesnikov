using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public interface IMerchDeliveryRepository : IRepository<MerchDelivery>
    {
        Task<IEnumerable<MerchDelivery>?> GetAsync(int employeeId, CancellationToken token = default);
    }
}