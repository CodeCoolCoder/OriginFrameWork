namespace OriginFrameWork.RemoteInvokeModule.RemoteServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<Dictionary<string, string>> ResolveServiceAddressAsync(string serviceName);
    }
}
