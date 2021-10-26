using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
#pragma warning disable 1591

namespace OzonEdu.MerchandiseApi.Infrastructure.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            _logger.LogInformation(requestJson);
            
            var response = base.UnaryServerHandler(request, context, continuation);

            var responseJson = JsonConvert.SerializeObject(response);
            _logger.LogInformation(responseJson);
            
            return response;
        }
    }
}