using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate
{
    public class RequestStatus : Enumeration
    {
        public static RequestStatus WasArrival = new(1, nameof(WasArrival));
        public static RequestStatus Done = new(2, nameof(Done));
        public static RequestStatus AutoPending = new(1, nameof(AutoPending));
        
        public RequestStatus(int id, string name) : base(id, name)
        {
        }
    }
}