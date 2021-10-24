using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace OzonEdu.MerchandiseApi.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next) { }

        public async Task InvokeAsync(HttpContext context)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var version = assemblyName.Version?.ToString() ?? "no version";
            var serviceName = assemblyName.Name ?? "no name";
            var result = new { version, serviceName };
            await context.Response.WriteAsync( JsonConvert.SerializeObject(result));
        }
    }
}