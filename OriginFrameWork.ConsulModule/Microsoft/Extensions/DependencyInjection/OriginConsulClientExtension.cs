using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using OriginFrameWork.ConsulModule.ConsulOptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class OriginConsulClientExtension
{
    /// <summary>
    /// 使用Consul
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="serviceProvider"></param> <summary>
    public static void UseConsul(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var consulClient = serviceProvider.GetRequiredService<IConsulClient>();
        RegisterConsul(configuration, consulClient);
    }
    /// <summary>
    /// 注册并获取相关consul配置文件
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="consulClient"></param>
    private static void RegisterConsul(IConfiguration configuration, IConsulClient consulClient)
    {
        var consulClientOption = new ConsulClientOption();
        //从配置文件绑定配置
        configuration.Bind("Consul:ConsulClient", consulClientOption);
        //所属服务组
        string consulGroup = consulClientOption.ServiceGroup;
        //服务地址
        int port = consulClientOption.Port;
        string ip = consulClientOption.IP;
        string servicerName = $"{consulGroup}_{ip}_{port}";
        var httpscheme = consulClientOption.IsHttps ? "https" : "http";
        var check = new AgentServiceCheck
        {
            Interval = TimeSpan.FromSeconds(consulClientOption.Interval), // 健康检测间隔时间
            HTTP = $"{httpscheme}://{ip}:{port}{consulClientOption.HealthUrl}", // 健康检测地址
            Timeout = TimeSpan.FromSeconds(consulClientOption.Timeout), // 请求后2秒内未响应，则认为服务以停止
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulClientOption.DeregisterCriticalServiceAfter), // 服务停止后2秒后注销服务                                                                    // TTL = TimeSpan.FromSeconds(2), 这个服务过多久自动失效

        };
        var regist = new AgentServiceRegistration
        {
            Check = check,
            Address = ip,
            ID = servicerName,
            Name = consulGroup,
            Port = port,
        };
        consulClient.Agent.ServiceRegister(regist);
    }

}
