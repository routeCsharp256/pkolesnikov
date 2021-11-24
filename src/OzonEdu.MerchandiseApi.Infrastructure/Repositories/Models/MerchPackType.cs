using System.Collections.Generic;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public record MerchPackType
    {
        public int Id { get; init; }
        
        public string? Name { get; init; }

        public List<int> MerchTypeIds { get; } = new();
    }
}