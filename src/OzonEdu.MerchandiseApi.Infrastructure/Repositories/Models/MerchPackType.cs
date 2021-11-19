namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public class MerchPackType
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public int[]? MerchTypeIds { get; set; }
    }
}