using Dapper;
using Grpc.Net.Client;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTracing;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Contracts;
using OzonEdu.MerchandiseApi.Domain.Events;
using OzonEdu.MerchandiseApi.Infrastructure.AppealProcessors;
using OzonEdu.MerchandiseApi.Infrastructure.Configuration;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.DomainEvent;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Queries;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Implementation;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Tracers;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchandiseApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IRequestHandler<GiveOutMerchCommand, Unit>,
                    GiveOutMerchHandler>()
                .AddScoped<IRequestHandler<GetMerchDeliveryStatusQuery, string>,
                    GetMerchDeliveryStatusQueryHandler>()
                .AddScoped<INotificationHandler<StockReplenishedDomainEvent>,
                    StockReplenishedDomainEventHandler>()
                .AddScoped<INotificationHandler<EmployeeNotificationDomainEvent>,
                    EmployeeNotificationDomainEventHandler>();
        }

        public static IServiceCollection AddDatabaseComponents(this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(DatabaseConnectionOptions));
            return services
                .Configure<DatabaseConnectionOptions>(section)
                .AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IChangeTracker, ChangeTracker>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return services
                .AddScoped<IEmployeeRepository, EmployeeRepository>()
                .AddScoped<IMerchDeliveryRepository, MerchDeliveryRepository>();
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IEmployeeService, EmployeeService>()
                .AddScoped<IMerchService, MerchService>()
                .AddScoped<IStockService, StockService>();
        }
        
        public static IServiceCollection AddJaegerTracer(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITracer>(serviceProvider =>
                {
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    var reporter = new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .WithSender(new UdpSender())
                        .Build();

                    var tracer = new Tracer.Builder("MerchApi")
                        .WithSampler(new ConstSampler(true))
                        .WithReporter(reporter)
                        .Build();

                    return tracer;
                })
                .AddSingleton<CustomTracer>();
        }

        public static IServiceCollection AddStockApiGrpcClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            var address = configuration
                .GetSection(nameof(StockApiGrpcClientConfiguration))
                .Get<StockApiGrpcClientConfiguration>()
                .Address;

            return services
                .AddScoped(_ =>
                {
                    var channel = GrpcChannel.ForAddress(address);
                    return new StockApiGrpc.StockApiGrpcClient(channel);
                });
        }

        public static IServiceCollection AddAppealProcessors(this IServiceCollection services)
        {
            return services
                .AddScoped<ManualAppealProcessor>()
                .AddScoped<AutoAppealProcessor>();
        }
    }
}