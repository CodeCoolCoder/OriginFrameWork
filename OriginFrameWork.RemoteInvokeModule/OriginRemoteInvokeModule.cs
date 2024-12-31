using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.Core.RemoteServiceDiscovery;
using OriginFrameWork.CoreModule;
using OriginFrameWork.CoreModule.Extensions;
using OriginFrameWork.RemoteInvokeModule.DynamicProxy;
using OriginFrameWork.RemoteInvokeModule.RemoteServerTodo;
using OriginFrameWork.RemoteInvokeModule.RemoteServiceDiscovery;

namespace OriginFrameWork.RemoteInvokeModule
{
    public class OriginRemoteInvokeModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            var configuration = context.Services.GetConfiguration();
            services.AddHttpClient();
            services.AddSingleton<IServiceDiscovery>(new ServiceDiscovery(configuration));
            services.AddSingleton<IRemoteServiceInvoker, RemoteServiceInvoker>();
            services.AddSingleton<RemoteServiceProxyGenerator>();
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> clientRemoteType = new List<Type>();
            foreach (var assembly in assemblys)
            {
                assembly.GetTypes().Where(t => t.IsInterface).ToList().ForEach(m =>
                                {
                                    var typename = m.GetInterface("IRemoteServiceTag");
                                    if (typename != null && typename.Name == "IRemoteServiceTag")
                                    {
                                        clientRemoteType.Add(m);
                                    }
                                });
                foreach (var type in clientRemoteType)
                {
                    services.AddTransient(type, sp =>
                    {
                        var generator = sp.GetRequiredService<RemoteServiceProxyGenerator>();
                        return generator.CreateProxy(type);

                    });
                }
                var serverRemoteType = assembly.GetTypes().Where(
                        m => m.IsAssignableTo(typeof(IRemoteServiceTag))
                        && !m.IsAbstract
                        && !m.IsInterface
                    );
                //注册controller动态生成模块
                foreach (var item in serverRemoteType)
                {
                    //注册controller动态生成模块
                    var interfacetype = item.GetInterfaces().FirstOrDefault();
                    //services.AddTransient(interfacetype, item);
                    var generator = new DynamicApiControllergenrator(services, "app/api");
                    generator.RegisterService(interfacetype);
                    services.AddScoped(interfacetype, item);
                }


                clientRemoteType.Clear();
            }
        }
    }
}
