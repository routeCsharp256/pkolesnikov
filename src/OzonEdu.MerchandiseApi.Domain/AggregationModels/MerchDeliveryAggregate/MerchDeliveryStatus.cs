using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate
{
    public class MerchDeliveryStatus : Enumeration
    {
        public static MerchDeliveryStatus EmployeeCame = new(1, nameof(EmployeeCame));
        public static MerchDeliveryStatus Done = new(2, nameof(Done));
        public static MerchDeliveryStatus Notify = new(3, nameof(Notify));
        public static MerchDeliveryStatus InWork = new(4, nameof(InWork));

        public MerchDeliveryStatus(int id, string name) : base(id, name)
        { }
    }
}