using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OriginFrameWork.ConsulModule.ConsulOptions;

namespace OriginFrameWork.ConsulModule.Microsoft.Extensions.DependencyInjection;

public static class OriginConsulCheckHealthyExtension
{
    /// <summary>
    ///健康检查
    /// </summary>
    /// <param name="webApplication"></param>
    public static void AddCheckHealth(this WebApplication webApplication)
    {
        var consulclient = new ConsulClientOption();
        webApplication.Configuration.Bind("Consul:ConsulClient", consulclient);
        webApplication.MapGet(consulclient.HealthUrl, () =>
        {
            return new OkResult();
        });
    }
}
