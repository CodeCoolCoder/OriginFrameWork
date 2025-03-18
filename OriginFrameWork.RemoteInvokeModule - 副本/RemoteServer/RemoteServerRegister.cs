using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule.Extensions;
using OriginFrameWork.CoreModule.OriginInterface;

namespace OriginFrameWork.RemoteInvokeModule.RemoteServer
{
    public static class RemoteServerRegister
    {
        /// <summary>
        /// 远程服务服务端注册
        /// </summary>
        /// <param name="services"></param>

        public static void RemoteServerRegisterModule(this IServiceCollection services)
        {
            var config = services.GetConfiguration();

            var baseUrls = config.GetSection("RemoteServices").GetChildren().ToList();
            //foreach (var baseUrl in baseUrls)
            //{
            //    Dictionary<string, string> serviceAddressSection = new Dictionary<string, string>();
            //    foreach (var item in baseUrl.GetChildren().ToList())
            //    {
            //        serviceAddressSection.Add(item.Key, item.Value);
            //    }
            //    _serviceAddresses.Add(baseUrl.Key, serviceAddressSection);
            //}
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblys)
            {
                var remoteType = assembly.GetTypes().Where(
                m => m.IsAssignableTo(typeof(IRemoteServiceTag))
                && !m.IsAbstract
                && !m.IsInterface
            );
                //注册controller动态生成模块
                foreach (var item in remoteType)
                {
                    var interfacetype = item.GetInterfaces().FirstOrDefault();
                    var keyName = item.Name;
                    var prefixName = baseUrls.Where(m => m.Key == keyName).FirstOrDefault().GetSection("Prefix").Value;
                    //services.AddTransient(interfacetype, item);
                    var generator = new DynamicApiControllergenrator(services, prefixName);
                    generator.RegisterService(interfacetype);
                    services.AddScoped(interfacetype, item);
                }
            }
        }


    }
}
