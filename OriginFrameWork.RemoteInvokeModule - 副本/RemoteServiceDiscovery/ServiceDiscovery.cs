using Microsoft.Extensions.Configuration;

namespace OriginFrameWork.RemoteInvokeModule.RemoteServiceDiscovery
{
    /// <summary>
    /// 远程服务发现
    /// </summary>
    public class ServiceDiscovery : IServiceDiscovery
    {
        private readonly static Dictionary<string, Dictionary<string, string>> _serviceAddresses = new Dictionary<string, Dictionary<string, string>>();
        public ServiceDiscovery(IConfiguration configuration)
        {
            //  _serviceAddresses = serviceAddresses;
            //读取配置文件拼接address
            Configuration = configuration;
            var baseUrls = Configuration.GetSection("RemoteServices").GetChildren().ToList();
            foreach (var baseUrl in baseUrls)
            {
                Dictionary<string, string> serviceAddressSection = new Dictionary<string, string>();
                foreach (var item in baseUrl.GetChildren().ToList())
                {
                    serviceAddressSection.Add(item.Key, item.Value);
                }
                _serviceAddresses.Add(baseUrl.Key, serviceAddressSection);
            }
        }
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 根据服务名称解析获取服务地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<Dictionary<string, string>> ResolveServiceAddressAsync(string serviceName)
        {
            if (_serviceAddresses.TryGetValue(serviceName, out var address))
            {
                //address
                return Task.FromResult(address);
            }
            throw new KeyNotFoundException($"Service {serviceName} not found");
        }
    }
}
