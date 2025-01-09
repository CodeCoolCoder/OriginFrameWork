using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule;
using OriginFrameWork.CoreModule.Extensions;
using OriginFrameWork.CoreModule.OriginInterface;
using OriginFrameWork.RemoteInvokeModule.DynamicProxy;
using OriginFrameWork.RemoteInvokeModule.RemoteServer;
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
            IEnumerable<Type> remoteType = Enumerable.Empty<Type>();
            foreach (var assembly in assemblys)
            {
                remoteType = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IRemoteServiceTag)) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in remoteType)
                {
                    var Itype = type.GetInterfaces().FirstOrDefault();
                    // var ss = Activator.CreateInstance(Itype);
                    services.AddTransient(Itype, sp =>
                    {
                        var generator = sp.GetRequiredService<RemoteServiceProxyGenerator>();
                        var proxyType = typeof(RemoteServiceProxyGenerator).GetMethod("CreateProxy").MakeGenericMethod(Itype);
                        return proxyType.Invoke(generator, null);
                    });
                }
            }
            //服务端注册
            services.RemoteServerRegisterModule();

        }
    }
}
