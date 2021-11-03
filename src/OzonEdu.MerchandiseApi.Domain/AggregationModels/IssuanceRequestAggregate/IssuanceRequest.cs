using System;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseApi.Domain.Models;

namespace OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate
{
    public class IssuanceRequest : Entity
    {
        public RequestNumber RequestNumber { get; }
        
        public RequestStatus RequestStatus { get; }
        
        public NewStatusDate NewStatusDate { get; private set; }

        public IssuanceRequest(RequestNumber number, RequestStatus status, NewStatusDate? date)
        {
            RequestNumber = number;
            RequestStatus = status;
            SetNewStatusDate(date);
        }

        public void SetNewStatusDate(NewStatusDate? date)
        {
            NewStatusDate = date ?? new NewStatusDate(DateTime.UtcNow);
        }
    }
}