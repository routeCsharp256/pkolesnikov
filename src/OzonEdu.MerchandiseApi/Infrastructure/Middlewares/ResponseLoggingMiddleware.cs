﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseApi.Constants;

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private const int HeaderNameSpace = -30;
        
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
            var path = response.HttpContext.Request.Path.Value;

            if (path is null || !path.StartsWith(RouteConstant.Route))
                return;
            
            await Task.Run(() =>
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("Response logged");
                stringBuilder.AppendLine($"Route - {path}");
                stringBuilder.AppendLine("Headers:");

                var headersString = response
                    .Headers
                    .Select(h => $"\t{h.Key, HeaderNameSpace}{h.Value}");
                
                stringBuilder.Append(string.Join("\n", headersString));

                _logger.LogInformation(stringBuilder.ToString());
            });
        }
    }
}