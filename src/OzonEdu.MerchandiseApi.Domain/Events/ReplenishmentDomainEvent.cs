using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class ReplenishmentDomainEvent : INotification
    {
        public IEnumerable<Sku> MerchItemSkuCollection { get; }

        public ReplenishmentDomainEvent(IEnumerable<Sku> collection)
        {
            MerchItemSkuCollection = collection;
        }
    }
}