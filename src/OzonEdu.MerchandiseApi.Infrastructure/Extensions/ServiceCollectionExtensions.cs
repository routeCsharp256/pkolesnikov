using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Contracts;
using OzonEdu.MerchandiseApi.Infrastructure.Configuration;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Handlers.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.MediatR.Queries;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Implementation;
using OzonEdu.MerchandiseApi.Infrastructure.Services.Interfaces;

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
                    GetMerchDeliveryStatusQueryHandler>();
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
                .AddScoped<IMerchService, MerchService>();
        }
    }
}