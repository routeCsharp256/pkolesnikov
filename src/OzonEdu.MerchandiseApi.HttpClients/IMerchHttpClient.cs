using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<GetMerchResponse?> GetMerch(GetMerchDeliveryStatusViewModel requestStatus, CancellationToken token);
        Task<GetMerchIssuanceResponse?> GetMerchIssuance(GetMerchDeliveryStatusViewModel requestStatus, CancellationToken token);
    }
}