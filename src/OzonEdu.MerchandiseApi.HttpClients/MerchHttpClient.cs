using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseApi.HttpModels;

namespace OzonEdu.MerchandiseApi.HttpClients
{
    public class MerchHttpClient : IMerchHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public MerchHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<GetMerchResponse?> GetMerch(GetMerchDeliveryStatusViewModel requestStatus, CancellationToken token)
        {
            var requestUri = $"v1/api/merch?id={requestStatus.EmployeeId}";
            using var response = await _httpClient.GetAsync(requestUri, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<GetMerchResponse>(body, options);
        }

        public async Task<GetMerchIssuanceResponse?> GetMerchIssuance(GetMerchDeliveryStatusViewModel requestStatus, CancellationToken token)
        {
            var requestUri = $"v1/api/merch/issuance?id={requestStatus.EmployeeId}";
            using var response = await _httpClient.GetAsync(requestUri, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<GetMerchIssuanceResponse>(body, options);
        }
    }
}