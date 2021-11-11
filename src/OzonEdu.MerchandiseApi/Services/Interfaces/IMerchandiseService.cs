using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Grpc;

namespace OzonEdu.MerchandiseApi.Services.Interfaces
{
    public interface IMerchandiseService
    {
        Task GiveOutMerch(GiveOutMerchRequest request, CancellationToken token);
        
        Task<GetMerchDeliveryStatusResponse> GetMerchDeliveryStatus(GetMerchDeliveryStatusRequest request, 
            CancellationToken token);
    }
}