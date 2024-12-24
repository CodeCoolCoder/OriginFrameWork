using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OriginFrameWork.CoreModule.Extensions
{
    /// <summary>
    /// 获取service相关配置扩展方法
    /// </summary>
    public static class OriginServiceConfigurationExtensions
    {
        //public static IServiceCollection ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration)
        //{
        //    return services.Replace(ServiceDescriptor.Singleton(configuration));
        //}

        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetConfigurationOrNull() ?? throw new Exception("Could not find an implementation of " + typeof(IConfiguration).AssemblyQualifiedName + " in the service collection.");
        }

        public static IConfiguration? GetConfigurationOrNull(this IServiceCollection services)
        {
            HostBuilderContext singletonInstanceOrNull = services.GetSingletonInstanceOrNull<HostBuilderContext>();
            if (singletonInstanceOrNull?.Configuration != null)
            {
                return singletonInstanceOrNull.Configuration as IConfigurationRoot;
            }

            return services.GetSingletonInstanceOrNull<IConfiguration>();
        }

    }
}
