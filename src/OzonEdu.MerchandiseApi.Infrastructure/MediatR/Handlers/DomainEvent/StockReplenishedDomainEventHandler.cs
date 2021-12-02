using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.DomainEvent
{
    public class StockReplenishedDomainEventHandler : INotificationHandler<StockReplenishedDomainEvent>
    {
        private readonly ILogger<StockReplenishedDomainEventHandler> _logger;
        private readonly AutoAppealProcessor _autoAppealProcessor;
        private readonly ManualAppealProcessor _manualAppealProcessor;

        public StockReplenishedDomainEventHandler(ILogger<StockReplenishedDomainEventHandler> logger,
            AutoAppealProcessor autoAppealProcessor,
            ManualAppealProcessor manualAppealProcessor)
        {
            _manualAppealProcessor = manualAppealProcessor;
            _autoAppealProcessor = autoAppealProcessor;
            _logger = logger;
        }
        
        public async Task Handle(StockReplenishedDomainEvent notification, CancellationToken token)
        {
            var skuCollection = notification
                .Items
                .Select(it => it.Sku)
                .ToArray();

            await _manualAppealProcessor.Do(skuCollection, token);
            await _autoAppealProcessor.Do(skuCollection, token);
        }
    }
}