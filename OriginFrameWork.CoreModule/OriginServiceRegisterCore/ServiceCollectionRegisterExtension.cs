using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule.OriginInterface;
using System.Reflection;



namespace OriginFrameWork.CoreModule.OriginServiceRegisterCore;

public static class ServiceCollectionRegisterExtension
{
    public static HashSet<Type> processedConfigModules = new();
    public static HashSet<Type> processedApplicationModules = new();
    public static IEnumerable<Type> Types = new List<Type>();
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
            var managertypes = asmService.GetTypes().Where(
                m => m.IsAssignableTo(typeof(IocManager))
                && !m.IsAbstract
                && !m.IsInterface
            );
            var managertypesForGeneric = asmService.GetTypes().Where(
              m => m.IsAssignableTo(typeof(IocManagerForGenerics))
              && !m.IsAbstract
              && !m.IsInterface
          );

            //     var cscs = asmService.GetTypes().Where(
            //        m => m.IsAssignableTo(typeof(IRemoteServiceTag))
            //        && !m.IsAbstract
            //        && !m.IsInterface
            //    );
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
            foreach (var manager in managertypes)
            {
                //var interfacetype = implementtationtypeForGeneric.GetGenericTypeDefinition();
                services.AddTransient(manager);
            }
            foreach (var managerForGeneric in managertypesForGeneric)
            {
                //var interfacetype = implementtationtypeForGeneric.GetGenericTypeDefinition();
                services.AddTransient(managerForGeneric);
            }


        }
        return services;
    }
    private static List<Type> GetTypes()
    {
        List<Type> types = new();
        var assemblys = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblys)
        {
            types.AddRange(assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IOriginModule)) && !t.IsInterface && !t.IsAbstract));

        }
        Types = types;
        return types;
    }
    /// <summary>
    ///扩展模块注册
    /// </summary>
    /// <param name="services"></param>
    public static void OriginModuleConfigRegister(this IServiceCollection services)
    {
        var context = new OriginServiceConfigurationContext(services);
        var types = GetTypes();
        foreach (var type in types)
        {
            //递归获取所有的模块
            ProcessConfigModule(type, context);
        }
        //var applicationContext = new OriginApplicationInitializationContext(application);
        // .GetAssemblies().GetReferanceAssemblies()
        //var assemblys = AppDomain.CurrentDomain.GetAssemblies();
        //foreach (var assembly in assemblys)
        //{
        //    var types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IOriginModule)) && !t.IsInterface && !t.IsAbstract);
        //    foreach (var type in types)
        //    {
        //        //递归获取所有的模块
        //        ProcessModule(type, context);
        //    }
        //}
    }
    public static void OriginModuleApplicationRegister(this WebApplication webApplication)
    {
        var context = new OriginApplicationInitializationContext(webApplication);

        foreach (var type in Types)
        {
            //递归获取所有的模块
            ProcessApplicationModule(type, context);
        }

    }
    /// <summary>
    /// 配置config模块
    /// </summary>
    /// <param name="moduleType"></param>
    /// <param name="context"></param> <summary>
    /// 
    /// </summary>
    /// <param name="moduleType"></param>
    /// <param name="context"></param>
    private static void ProcessConfigModule(Type moduleType, OriginServiceConfigurationContext? context)
    {
        // 如果已经处理过这个模块，直接返回
        if (!processedConfigModules.Add(moduleType))
        {
            return;
        }

        // 先处理依赖模块
        var attributes = moduleType.GetCustomAttributes<OriginInject>(true);
        foreach (var attr in attributes)
        {
            foreach (var dependencyType in attr.ModuleType)
            {
                // 递归处理依赖模块
                ProcessConfigModule(dependencyType, context);

            }
        }

        // 创建并配置当前模块
        try
        {
            var moduleInstance = Activator.CreateInstance(moduleType) as IOriginModule;
            if (moduleInstance != null)
            {

                moduleInstance.ConfigureServices(context);

                Console.WriteLine($"Config已配置模块: {moduleType.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Config创建模块 {moduleType.Name} 失败: {ex.Message}");
        }
    }
    /// <summary>
    /// 配置application模块
    /// </summary>
    /// <param name="moduleType"></param>
    /// <param name="applicationInitializationContext"></param> <summary>
    private static void ProcessApplicationModule(Type moduleType, OriginApplicationInitializationContext? applicationInitializationContext)
    {
        // 如果已经处理过这个模块，直接返回
        if (!processedApplicationModules.Add(moduleType))
        {
            return;
        }

        // 先处理依赖模块
        var attributes = moduleType.GetCustomAttributes<OriginInject>(true);
        foreach (var attr in attributes)
        {
            foreach (var dependencyType in attr.ModuleType)
            {
                // 递归处理依赖模块
                ProcessApplicationModule(dependencyType, applicationInitializationContext);

            }
        }

        // 创建并配置当前模块
        try
        {
            var moduleInstance = Activator.CreateInstance(moduleType) as IOriginModule;
            if (moduleInstance != null)
            {

                moduleInstance.ApplicationInitialization(applicationInitializationContext);
                Console.WriteLine($"Application已配置模块: {moduleType.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Application创建模块 {moduleType.Name} 失败: {ex.Message}");
        }
    }

}



