using System;
using System.Linq;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseApi.Domain.Tests
{
    public class MerchPackTests
    {
        [Fact]
        public void SetInitiatingEventName_NullEventName_ArgumentNullException()
        {
            var merchPack = new MerchPack(
                new MerchPackId(1),
                MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(x => x == MerchPackType.Starter));

            Assert.Throws<ArgumentNullException>(() => merchPack.SetInitiatingEventName(null));
        }
        
        [Fact]
        public void SetInitiatingEventNameForListener_NewEventName_Success()
        {
            var merchPack = new MerchPack(
                new MerchPackId(1),
                MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(x => x == MerchPackType.ConferenceListener));

            var eventName = new InitiatingEventName("TestName");

            merchPack.SetInitiatingEventName(eventName);
            
            Assert.Equal(eventName, merchPack.InitiatingEventName);
        }
        
        [Fact]
        public void SetInitiatingEventNameForStarter_EventName_NullEventName()
        {
            var merchPack = new MerchPack(
                new MerchPackId(1),
                MerchPackType
                    .GetAll<MerchPackType>()
                    .FirstOrDefault(x => x == MerchPackType.Starter));

            merchPack.SetInitiatingEventName(new InitiatingEventName("TestName"));
            
            Assert.Null(merchPack.InitiatingEventName);
        }
    }
}