using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<GetMerchResponse?> GetMerch(GetIssuanceRequestStatusViewModel requestStatus, CancellationToken token);
        Task<GetMerchIssuanceResponse?> GetMerchIssuance(GetIssuanceRequestStatusViewModel requestStatus, CancellationToken token);
    }
}