using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;

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