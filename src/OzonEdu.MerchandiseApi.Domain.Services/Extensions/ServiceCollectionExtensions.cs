using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Implementation;
using OzonEdu.MerchandiseApi.Domain.Services.Contracts.Interfaces;
using OzonEdu.MerchandiseApi.Domain.Services.MediatR.Handlers.DomainEvent;

namespace OzonEdu.MerchandiseApi.Domain.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(EmployeeNotificationDomainEventHandler).Assembly);
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<IMerchService, MerchService>();
            return services;
        }
    }
}