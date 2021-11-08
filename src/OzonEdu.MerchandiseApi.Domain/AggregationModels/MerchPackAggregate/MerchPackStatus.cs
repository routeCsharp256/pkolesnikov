using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPackStatus : Enumeration
    {
        public static MerchPackStatus EmployeeCame = new(1, nameof(EmployeeCame));
        public static MerchPackStatus Done = new(2, nameof(Done));
        public static MerchPackStatus Notify = new(3, nameof(Notify));
        public static MerchPackStatus InWork = new(4, nameof(InWork));

        public MerchPackStatus(int id, string name) : base(id, name)
        { }
    }
}