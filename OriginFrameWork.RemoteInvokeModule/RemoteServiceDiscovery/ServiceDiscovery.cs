using Microsoft.Extensions.Configuration;

namespace OriginFrameWork.RemoteInvokeModule.RemoteServiceDiscovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        private readonly Dictionary<string, Dictionary<string, string>> _serviceAddresses = new Dictionary<string, Dictionary<string, string>>();
        public ServiceDiscovery(IConfiguration configuration)
        {
            //  _serviceAddresses = serviceAddresses;
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
