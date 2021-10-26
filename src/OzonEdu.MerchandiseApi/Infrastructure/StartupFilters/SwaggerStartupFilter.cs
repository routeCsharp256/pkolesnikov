using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
#pragma warning disable 1591

namespace OzonEdu.MerchandiseApi.Infrastructure.StartupFilters
{
    public class SwaggerStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                next(app);
            };
        }
    }
}