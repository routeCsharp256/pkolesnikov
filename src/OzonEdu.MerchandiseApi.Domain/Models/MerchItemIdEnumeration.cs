using System.Collections.Generic;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Models
{
    public class MerchItemIdEnumeration : Enumeration
    {
        public List<MerchItemId> MerchItemIds { get; } = new();
        
        public MerchItemIdEnumeration(int id, string name, IEnumerable<MerchItemId> items) : base(id, name)
        {
            MerchItemIds.AddRange(items);
        }
    }
}