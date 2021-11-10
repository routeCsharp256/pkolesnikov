using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public interface IMerchDeliveryRepository : IRepository<MerchDelivery>
    {
        Task<MerchDelivery?> FindByIdAsync(int id, CancellationToken token = default);

        Task<List<MerchDelivery>> GetAll(CancellationToken token = default);

        Task<List<MerchDelivery>> GetByStatus(int statusId, CancellationToken token = default);
    }
}