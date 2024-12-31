using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OriginFrameWork.CoreModule;
using OriginFrameWork.Entity;
using System.Reflection;


namespace OriginFrameWork.Core.Extensions;

public static class ServiceCollectionRegisterExtension
{
    /// <summary>
    /// 仓储注册
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RepositoryRegister(this IServiceCollection services)
    {
        //反射获取core中的仓储服务,load-->已知程序集的文件名或路径，加载程序集。
        var asmCore = Assembly.Load("OriginFrameWork.Core");
        //GetTypes，获取所有类型，在DealerPlatForm.Core程序集中获取泛型类型为Repository，泛型参数只有一个的泛型，`1表示只有一个泛型参数，我们的Repository只有一个泛型参数entity
        var implementtationtype = asmCore.GetTypes().FirstOrDefault(m => m.Name == "BaseRepository`1");
        //获取接口类型,GetGenericTypeDefinition返回一个表示可用于构造当前泛型类型的泛型类型定义的 Type 对象。 GetInterface返回的是一个对象，GetGenericTypeDefinition返回的是type对象
        var interfacetype = implementtationtype?.GetInterface("IBaseRepository`1").GetGenericTypeDefinition();

        //开始注册服务,因为implementtationtype和interfacetype都是type类型，所以同builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        if (interfacetype != null && implementtationtype != null)
        {
            services.AddTransient(interfacetype, implementtationtype);
        }
        return services;
    }


    /// <summary>
    /// 注册涉及到业务的一些服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ServiceRegister(this IServiceCollection services)
    {   //获取容器
        var provider = services.BuildServiceProvider();
        //从容器中获取IConfiguration
        var config = provider.GetService<IConfiguration>();
        List<string> iocLists = new();
        //从配置文件中读取程序集
        iocLists = config["IocList"].Split(",").ToList();
        List<Assembly> assemblies = new();
        foreach (var iocList in iocLists)
        {
            //反射获取core中的仓储服务,load-->已知程序集的文件名或路径，加载程序集。
            var asmService = Assembly.Load(iocList);
            assemblies.Add(asmService);
        }
        foreach (var asmService in assemblies)
        {
            //创建一个标记ioctag，所有的服务都继承于他，从他下面取服务即可
            var implementtationtypes = asmService.GetTypes().Where(
                m => m.IsAssignableTo(typeof(IocTag))
                && !m.IsAbstract
                && !m.IsInterface
            );
            var implementtationtypesForGeneric = asmService.GetTypes().Where(
                m => m.IsAssignableTo(typeof(IocTagForGenerics))
                && !m.IsAbstract
                && !m.IsInterface
            );
            // .Where(m => m != typeof(IocTag))
            ///对取到的接口进行循环，排除掉ioctag，剩下的就是我们要的,implementtationtypes里面只有一条数据，因为只有一个customerservice继承ioctag
            foreach (var implementtationtype in implementtationtypes)
            {
                var interfacetype = implementtationtype.GetInterfaces().FirstOrDefault();
                services.AddTransient(interfacetype, implementtationtype);
            }
            //注册泛型
            foreach (var implementtationtypeForGeneric in implementtationtypesForGeneric)
            {
                var interfacetype = implementtationtypeForGeneric.GetGenericTypeDefinition();
                services.AddTransient(interfacetype, implementtationtypeForGeneric);
            }
        }
        return services;
    }


    public static void OriginModuleRegister(this IServiceCollection services)
    {
        var context = new OriginServiceConfigurationContext(services);
        // .GetAssemblies().GetReferanceAssemblies()
        var assemblys = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblys)
        {

            var types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IOriginModule)) && !t.IsInterface && !t.IsAbstract);
            foreach (var type in types)
            {
                var nowModule = Activator.CreateInstance(type) as IOriginModule;
                nowModule.ConfigureServices(context);
                var attribute = type.GetCustomAttributes<OriginInject>(true).ToList();
                foreach (var attr in attribute)
                {
                    var origintypes = attr.ModuleType;
                    foreach (var item in origintypes)
                    {
                        var moduleRes = Activator.CreateInstance(item) as IOriginModule;
                        moduleRes.ConfigureServices(context);
                    }
                }

            }

        }


    }

}
