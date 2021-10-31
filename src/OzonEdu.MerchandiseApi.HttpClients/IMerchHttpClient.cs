using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<GetMerchResponse?> GetMerch(GetMerchRequest request, CancellationToken token);
        Task<GetMerchIssuanceResponse?> GetMerchIssuance(GetMerchIssuanceRequest request, CancellationToken token);
    }
}