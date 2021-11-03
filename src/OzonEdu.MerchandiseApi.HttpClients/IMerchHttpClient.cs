using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<GetMerchResponse?> GetMerch(IssuanceRequestViewModel request, CancellationToken token);
        Task<GetMerchIssuanceResponse?> GetMerchIssuance(IssuanceRequestViewModel request, CancellationToken token);
    }
}