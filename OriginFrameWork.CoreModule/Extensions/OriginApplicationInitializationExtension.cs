using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OriginFrameWork.CoreModule.OriginUtils;

namespace OriginFrameWork.CoreModule.Extensions
{
    public static class ApplicationInitializationContextExtensions
    {
        public static IApplicationBuilder GetApplicationBuilder(this OriginApplicationInitializationContext context)
        {
            var applicationBuilder = context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;

            return applicationBuilder;
        }

        public static IApplicationBuilder? GetApplicationBuilderOrNull(this OriginApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        }

        public static IWebHostEnvironment GetEnvironment(this OriginApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        }

        public static IWebHostEnvironment? GetEnvironmentOrNull(this OriginApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetService<IWebHostEnvironment>();
        }

        public static IConfiguration GetConfiguration(this OriginApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IConfiguration>();
        }

        public static ILoggerFactory GetLoggerFactory(this OriginApplicationInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<ILoggerFactory>();
        }
    }
}
