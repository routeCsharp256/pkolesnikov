using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchType : SizeEnumeration
    {
        public static MerchType TShirt = new(1, nameof(TShirt), true);
        public static MerchType Sweatshirt = new(2, nameof(Sweatshirt), true);
        public static MerchType Notepad = new(3, nameof(Notepad));
        public static MerchType Bag = new(4, nameof(Bag));
        public static MerchType Pen = new(5, nameof(Pen));
        public static MerchType Socks = new(6, nameof(Socks), true);

        public MerchType(int id, string name, bool hasSize = false) : base(id, name, hasSize) 
        { }
    }
}