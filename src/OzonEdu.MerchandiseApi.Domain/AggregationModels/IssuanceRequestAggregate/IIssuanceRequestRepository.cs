using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate
{
    public interface IIssuanceRequestRepository : IRepository<IssuanceRequest>
    {
        Task<IssuanceRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}