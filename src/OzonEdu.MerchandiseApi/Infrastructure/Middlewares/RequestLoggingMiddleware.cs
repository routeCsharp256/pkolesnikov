using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using OzonEdu.MerchandiseApi.Constants;
#pragma warning disable 1591

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context.Request);
            await _next(context);
        }

        private async Task LogRequest(HttpRequest request)
        {
            var path = request.Path.Value;

            if (path is null || !path.StartsWith(RouteConstant.Route))
                return;
            
            await Task.Run(() =>
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Request logged");
                stringBuilder.AppendLine($"Route - {path}");
                stringBuilder.AppendLine("Headers:");
                stringBuilder.AppendLine(request.Headers.ToString());
                _logger.LogInformation(stringBuilder.ToString());
            });
        }
    }
}