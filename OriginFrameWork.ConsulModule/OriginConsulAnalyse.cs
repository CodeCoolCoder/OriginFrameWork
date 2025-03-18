using Consul;
using Microsoft.Extensions.Configuration;

namespace OriginFrameWork.ConsulModule
{
    /// <summary>
    ///供其他微服务去consul中找同组的微服务，ocelot使用
    /// </summary>
    public class OriginConsulAnalyse : IOriginConsulAnalyse
    {
        public OriginConsulAnalyse(IConsulClient consulClient, IConfiguration configuration)
        {
            ConsulClient = consulClient;
            Configuration = configuration;
        }

        public IConsulClient ConsulClient { get; }
        public IConfiguration Configuration { get; }


        public string GetUrl(string routeUrl)
        {
            Uri uri = new Uri(routeUrl);
            var servicename = uri.Host;
            var servicescheme = uri.Scheme;
            var ipAndport = ConsulAnalyse(servicename);
            return $"{servicescheme}://{ipAndport}{uri.PathAndQuery}";
        }
        public string ConsulAnalyse(string serviceName)
        {
            //忽略大小写比较
            var response = ConsulClient.Agent.Services().Result.Response.Where(x => x.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).ToList();
            var index = new Random().Next(0, response.Count());
            var serviceInfo = response[index].Value;
            var url = $"{serviceInfo.Address}:{serviceInfo.Port}";
            return url;
        }
    }
}
