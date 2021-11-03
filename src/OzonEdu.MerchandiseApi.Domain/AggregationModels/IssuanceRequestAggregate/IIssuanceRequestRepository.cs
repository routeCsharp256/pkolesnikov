using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Domain.Contracts;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate
{
    public interface IIssuanceRequestRepository : IRepository<IssuanceRequest>
    {
        Task<IssuanceRequest?> FindByEmployeeIdAndMerchPackIdIdAsync(long employeeId, int packId, 
            CancellationToken cancellationToken = default);
    }
}