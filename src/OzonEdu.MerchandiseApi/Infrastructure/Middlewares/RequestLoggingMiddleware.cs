using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using OzonEdu.MerchandiseApi.Constants;

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private const int HeaderNameSpace = -30;
        
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

            if (path is null || !path.StartsWith(RouteConstant.MerchRoute))
                return;
            
            await Task.Run(() =>
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Route - {path}");
                stringBuilder.AppendLine("Headers:");

                var headersString = request
                    .Headers
                    .Select(h => $"\t{h.Key, HeaderNameSpace}{h.Value}\n");
                
                stringBuilder.Append(string.Concat(headersString));

                _logger.LogInformation(stringBuilder.ToString());
            });
        }
    }
}