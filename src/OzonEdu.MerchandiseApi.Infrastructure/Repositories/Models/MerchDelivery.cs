using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models
{
    public record MerchDelivery
    {
        public int MerchDeliveryId { get; init; }
        public int? MerchPackTypeId { get; init; }
        public int? MerchDeliveryStatusId { get; init; }
        public DateTime? StatusChangeDate { get; init; }
        public List<long> SkuCollection { get; } = new();
    }
}