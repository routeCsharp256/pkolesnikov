using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<GetMerchResponse?> GetMerch(GetMerchDeliveryStatusRequest requestStatus, CancellationToken token);
        Task<GetMerchIssuanceResponse?> GetMerchIssuance(GetMerchDeliveryStatusRequest requestStatus, CancellationToken token);
    }
}