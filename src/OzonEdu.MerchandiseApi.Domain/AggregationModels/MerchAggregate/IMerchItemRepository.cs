using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public interface IMerchItemRepository : IRepository<MerchItem>
    {
        Task<MerchItem> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<MerchItem> FindBySkuAsync(Sku sku, CancellationToken cancellationToken = default);
    }
}