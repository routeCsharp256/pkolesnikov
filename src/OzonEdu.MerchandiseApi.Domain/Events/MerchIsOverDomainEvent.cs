using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class MerchIsOverDomainEvent : INotification
    {
        public IEnumerable<Sku> MerchItemSkuCollection { get; }

        public MerchIsOverDomainEvent(IEnumerable<Sku> collection)
        {
            MerchItemSkuCollection = collection;
        }
    }
}