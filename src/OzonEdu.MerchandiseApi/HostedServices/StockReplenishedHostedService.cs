using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseApi.HostedServices
{
    public class StockReplenishedHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockReplenishedHostedService> _logger;
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly IMediator _mediator;

        public StockReplenishedHostedService(IServiceScopeFactory scopeFactory,
            ILogger<StockReplenishedHostedService> logger,
            IOptions<KafkaConfiguration> options,
            IMediator mediator)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _kafkaConfiguration = options.Value;
            _mediator = mediator;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO Нужна проверка, не пустая ли конфигурация
            var consumerConfig = new ConsumerConfig
            {
                GroupId = _kafkaConfiguration.GroupId,
                BootstrapServers = _kafkaConfiguration.BootstrapServers
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_kafkaConfiguration.StockReplenishedEventTopic);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();
                    try
                    {
                        await Task.Yield();
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult is null)
                            continue;

                        var serializedMessage = consumeResult.Message.Value;
                        var message = JsonSerializer.Deserialize<StockReplenishedEvent>(serializedMessage);
                        if (message is null)
                            continue;

                        await _mediator.Publish(new StockReplenishedDomainEvent(message), stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error when consume stock replenished event topic: {error}", ex.Message);
                    }
                }
            }
            finally
            {
                consumer.Commit();
                consumer.Close();
            }
        }
    }
}