using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.Infrastructure.Extensions;

#pragma warning disable 1591

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            await LogResponse(context.Response);
        }

        private async Task LogResponse(HttpResponse response)
        {
            try
            {
                var request = response
                    .HttpContext
                    .Request;
                
                var path = request
                    .Path
                    .Value;
                
                if (path is null || !path.StartsWith(RouteConstant.Route))
                    return;
                
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Response logged");
                stringBuilder.AppendLine($"Route: {request.GetRoute()}");
                stringBuilder.AppendLine("Headers:");
                stringBuilder.AppendLine(response.Headers.AsString());

                var body = await response.BodyToString();
                stringBuilder.Append($"Body: {body}");
                _logger.LogInformation(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not log response");
            }
        }
    }
}