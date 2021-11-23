using System;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public record MerchDelivery
    {
        public int Id { get; init; }
        public int? MerchPackTypeId { get; init; }
        public int? MerchDeliveryStatusId { get; init; }
        public DateTime? StatusChangeDate { get; init; }
        public long[]? SkuCollection { get; init; }
    }
}