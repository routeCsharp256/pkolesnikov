using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
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
        
        public async Task<GetMerchResponse?> GetMerch(GetMerchRequest request, CancellationToken token)
        {
            var uri = new UriBuilder("v1/api/merch");
            var name = nameof(request.Id).ToLower();
            var value = request.Id.ToString();
            uri.Query = QueryHelpers.AddQueryString(uri.Query, name, value);
            using var response = await _httpClient.GetAsync(uri.Query, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<GetMerchResponse>(body);
        }

        public async Task<GetMerchIssuanceResponse?> GetMerchIssuance(GetMerchIssuanceRequest request, CancellationToken token)
        {
            using var response = await _httpClient.GetAsync("v1/api/merchandise/issuance?id=23", token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<GetMerchIssuanceResponse>(body);
        }
    }
}