using System;

namespace OzonEdu.MerchandiseApi.Domain.Exceptions.IssuanceRequestAggregate
{
    public class AlreadyDoneIssuanceRequest : Exception
    {
        public AlreadyDoneIssuanceRequest(string message) : base(message)
        {}
        
        public AlreadyDoneIssuanceRequest(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
}