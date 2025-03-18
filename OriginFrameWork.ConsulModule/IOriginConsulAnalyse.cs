using Consul;
using Microsoft.Extensions.Configuration;

namespace OriginFrameWork.ConsulModule
{
    public interface IOriginConsulAnalyse
    {
        string GetUrl(string routeUrl);

    }
}
