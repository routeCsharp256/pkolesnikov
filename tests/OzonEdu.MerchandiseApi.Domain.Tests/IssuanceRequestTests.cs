using OzonEdu.MerchandiseApi.Domain.AggregationModels.IssuanceRequestAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseApi.Domain.Exceptions.IssuanceRequestAggregate;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class IssuanceRequestTests
    {
        [Fact]
        public void SetRequestStatus_NotDoneStatus_Success()
        {
            var request = new IssuanceRequest(
                new EmployeeId(1),
                new MerchPackId(1),
                new RequestNumber(1));
            
            request.SetRequestStatus(RequestStatus.InWork);
            
            Assert.Equal(RequestStatus.InWork, request.MerchPackStatus);
        }
        
        [Fact]
        public void SetRequestStatus_RequestInDoneStatus_AlreadyDoneIssuanceRequestException()
        {
            var request = new IssuanceRequest(
                new EmployeeId(1),
                new MerchPackId(1),
                new RequestNumber(1));
            request.SetRequestStatus(RequestStatus.Done);
            
            Assert.Throws<MerchDeliveryAlreadyDone>(() => request.SetRequestStatus(RequestStatus.InWork));
        }
    }
}