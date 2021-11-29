using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace OzonEdu.MerchandiseApi.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddJaegerTracer(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITracer>(serviceProvider =>
                {
                    var serviceName = serviceProvider
                        .GetRequiredService<IWebHostEnvironment>()
                        .ApplicationName;

                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    var reporter = new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .WithSender(new UdpSender())
                        .Build();

                    var tracer = new Tracer.Builder(serviceName)
                        .WithSampler(new ConstSampler(true))
                        .WithReporter(reporter)
                        .Build();

                    return tracer;
                });
        }
    }
}