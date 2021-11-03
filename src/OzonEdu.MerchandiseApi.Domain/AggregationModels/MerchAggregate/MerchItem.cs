using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate
{
    public class MerchItem : Entity
    {
        public MerchItemId MerchItemId { get; }
        
        public MerchName MerchName { get; }
        
        public Sku? Sku { get; set; }
        
        public ClothingSize? ClothingSize { get; set; }
        
        public MerchType MerchType { get; }

        public MerchItem(MerchItemId id,
            MerchName merchName,
            MerchType merchType)
        {
            MerchItemId = id;
            MerchName = merchName;
            MerchType = merchType;
        }
        
        public void SetClothingSize(ClothingSize? size)
        {
            if (MerchType.HasSize && size is not null)
                ClothingSize = size;
        }

        public void SetSku(Sku? sku) => Sku = sku;
    }
}