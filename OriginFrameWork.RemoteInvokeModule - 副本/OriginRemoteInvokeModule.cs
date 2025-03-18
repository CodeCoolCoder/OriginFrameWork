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
            //客户端拦截转发
            foreach (var assembly in assemblys)
            {
                //remoteType = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IRemoteServiceTag)) && !t.IsInterface && !t.IsAbstract);
                remoteType = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IRemoteServiceTag)));
                foreach (var type in remoteType)
                {
                    // var Itype = type.GetInterfaces().FirstOrDefault();
                    // var ss = Activator.CreateInstance(Itype);
                    services.AddTransient(type, sp =>
                    {
                        var generator = sp.GetRequiredService<RemoteServiceProxyGenerator>();
                        //.MakeGenericMethod(Itype)
                        var proxyType = typeof(RemoteServiceProxyGenerator).GetMethod("CreateProxy");
                        return proxyType.Invoke(generator, new object[] { type });
                    });
                }
            }
            //服务端注册
            services.RemoteServerRegisterModule();

        }
    }
}
