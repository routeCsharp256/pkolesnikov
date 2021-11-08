using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class ReplenishmentDomainEvent : INotification
    {
        public IEnumerable<MerchType> MerchTypes { get; }

        public ReplenishmentDomainEvent(IEnumerable<MerchType> collection)
        {
            MerchTypes = collection;
        }
    }
}