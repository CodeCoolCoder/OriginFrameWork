using Microsoft.Extensions.DependencyInjection;
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
                    //services.AddTransient(interfacetype, item);
                    var generator = new DynamicApiControllergenrator(services, "app/api");
                    generator.RegisterService(interfacetype);
                    services.AddScoped(interfacetype, item);
                }
            }
        }


    }
}
