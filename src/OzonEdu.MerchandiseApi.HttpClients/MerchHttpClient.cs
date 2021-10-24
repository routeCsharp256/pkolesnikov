using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public class MerchHttpClient : IMerchHttpClient
    {
        private readonly HttpClient _httpClient;

        public MerchHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<GetMerchResponse> GetMerch(GetMerchRequest request, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GetMerchIssuanceResponse> GetMerchIssuance(GetMerchIssuanceRequest request, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}