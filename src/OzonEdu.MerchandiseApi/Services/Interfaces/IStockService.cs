using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.Grpc;
using GetMerchIssuanceResponse = OzonEdu.MerchandiseApi.Grpc.GetMerchIssuanceResponse;

namespace OzonEdu.MerchandiseApi.Services.Interfaces
{
    public interface IMerchandiseService
    {
        Task<GetMerchIssuanceRequest> GetMerch(GetMerchResponse response, CancellationToken token);
        
        Task<GetMerchIssuanceRequest> GetMerchIssuance(GetMerchIssuanceResponse response, CancellationToken _);
    }
}