using Consul;
using Microsoft.Extensions.Configuration;
using OriginFrameWork.ConsulModule;
using OriginFrameWork.ConsulModule.ConsulOptions;

namespace Microsoft.Extensions.DependencyInjection
{

    public static class OriginConsulServerExtension
    {
        /// <summary>
        /// 添加consul服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns> <summary>      
        public static IServiceCollection AddConsulServer(this IServiceCollection services)
        {
            var consulserveroptions = new ConsulServerOption();
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            configuration.Bind("Consul:ConsulServer", consulserveroptions);
            services.AddSingleton<IConsulClient>(m => new ConsulClient(Config =>
            {
                //"http://:8500";
                if (consulserveroptions.IsHttps)
                {
                    Config.Address = new Uri("https://" + consulserveroptions.IP + ":" + consulserveroptions.Port);
                }
                else
                {
                    Config.Address = new Uri("http://" + consulserveroptions.IP + ":" + consulserveroptions.Port);
                }
            })
           );
            return services;
        }

        // 定义一个扩展方法 AddConsulAnalyse，用于向 IServiceCollection 中添加服务
        public static IServiceCollection AddConsulAnalyse(this IServiceCollection services)
        {
            services.AddTransient<IOriginConsulAnalyse, OriginConsulAnalyse>();
            return services;
        }
    }

}
