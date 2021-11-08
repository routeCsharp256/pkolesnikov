using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class MerchIsOverDomainEvent : INotification
    {
        public IEnumerable<MerchType> MerchTypes { get; }

        public MerchIsOverDomainEvent(IEnumerable<MerchType> collection)
        {
            MerchTypes = collection;
        }
    }
}