namespace OzonEdu.MerchandiseApi.HttpModels
{
    public class GetMerchResponse
    {
        public string Description { get; set; }

        public GetMerchResponse(long id)
        {
            Description = $"Your ID is {id}";
        }

        public GetMerchResponse() { }
    }
}