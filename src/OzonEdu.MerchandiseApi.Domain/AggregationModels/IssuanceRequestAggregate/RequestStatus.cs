using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate
{
    public class RequestStatus : Enumeration
    {
        public static RequestStatus InWork = new(1, nameof(InWork));
        public static RequestStatus Done = new(2, nameof(Done));
        
        public RequestStatus(int id, string name) : base(id, name)
        {
        }
    }
}