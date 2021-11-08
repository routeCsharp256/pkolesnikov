using System;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class MerchPackTests
    {
        [Fact]
        public void SetInitiatingEventName_NullEventName_ArgumentNullException()
        {
            var merchPack = new MerchDelivery(
                new MerchPackId(1),
                MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(x => x == MerchPackType.ProbationPeriodEndingPack));

            Assert.Throws<ArgumentNullException>(() => merchPack.SetInitiatingEventName(null));
        }
        
        [Fact]
        public void SetInitiatingEventNameForListener_NewEventName_Success()
        {
            var merchPack = new MerchDelivery(
                new MerchPackId(1),
                MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(x => x == MerchPackType.ConferenceListenerPack));

            var eventName = new InitiatingEventName("TestName");

            merchPack.SetInitiatingEventName(eventName);
            
            Assert.Equal(eventName, merchPack.InitiatingEventName);
        }
        
        [Fact]
        public void SetInitiatingEventNameForStarter_EventName_NullEventName()
        {
            var merchPack = new MerchDelivery(
                new MerchPackId(1),
                MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(x => x == MerchPackType.ProbationPeriodEndingPack));

            merchPack.SetInitiatingEventName(new InitiatingEventName("TestName"));
            
            Assert.Null(merchPack.InitiatingEventName);
        }
    }
}