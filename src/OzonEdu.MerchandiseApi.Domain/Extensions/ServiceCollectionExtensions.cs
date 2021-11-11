using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;

namespace OzonEdu.MerchandiseApi.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IMerchDeliveryRepository, MerchDeliveryRepository>();
            return services;
        }
    }
}