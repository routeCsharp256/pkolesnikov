namespace OzonEdu.MerchandiseApi.HttpModels
{
    public class GetMerchIssuanceResponse
    {
        public string Description { get; set; }

        public GetMerchIssuanceResponse(long id)
        {
            Description = $"GetMerchIssuanceResponse: your ID is {id}";
        }

        public GetMerchIssuanceResponse() { }
    }
}