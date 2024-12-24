using Microsoft.Extensions.DependencyInjection;

namespace OriginFrameWork.CoreModule.Extensions
{
    public static class OriginServiceCommonExtensions
    {
        public static bool IsAdded<T>(this IServiceCollection services)
        {
            return services.IsAdded(typeof(T));
        }

        public static bool IsAdded(this IServiceCollection services, Type type)
        {
            Type type2 = type;
            return services.Any((ServiceDescriptor d) => d.ServiceType == type2);
        }

        //public static ITypeFinder GetTypeFinder(this IServiceCollection services)
        //{
        //    return services.GetSingletonInstance<ITypeFinder>();
        //}
        public static object? NormalizedImplementationInstance(this ServiceDescriptor descriptor) => descriptor.IsKeyedService ? descriptor.KeyedImplementationInstance : descriptor.ImplementationInstance;

        public static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)(services.FirstOrDefault((ServiceDescriptor d) => d.ServiceType == typeof(T))?.NormalizedImplementationInstance());
        }

        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            return services.GetSingletonInstanceOrNull<T>() ?? throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
        }

        //public static IServiceProvider BuildServiceProviderFromFactory(this IServiceCollection services)
        //{
        //    Check.NotNull(services, "services");
        //    foreach (ServiceDescriptor service in services)
        //    {
        //        Type type = service.NormalizedImplementationInstance()?.GetType().GetTypeInfo().GetInterfaces()
        //            .FirstOrDefault((Type i) => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));
        //        if (!(type == null))
        //        {
        //            Type type2 = type.GenericTypeArguments[0];
        //            return (IServiceProvider)typeof(ServiceCollectionCommonExtensions).GetTypeInfo().GetMethods().Single((MethodInfo m) => m.Name == "BuildServiceProviderFromFactory" && m.IsGenericMethod)
        //                .MakeGenericMethod(type2)
        //                .Invoke(null, new object[2] { services, null });
        //        }
        //    }

        //    return services.BuildServiceProvider();
        //}

        //public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>(this IServiceCollection services, Action<TContainerBuilder>? builderAction = null) where TContainerBuilder : notnull
        //{
        //    Check.NotNull(services, "services");
        //    IServiceProviderFactory<TContainerBuilder>? obj = services.GetSingletonInstanceOrNull<IServiceProviderFactory<TContainerBuilder>>() ?? throw new AbpException($"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {services}.");
        //    TContainerBuilder val = obj.CreateBuilder(services);
        //    builderAction?.Invoke(val);
        //    return obj.CreateServiceProvider(val);
        //}

        //
        // 摘要:
        //     Resolves a dependency using given Microsoft.Extensions.DependencyInjection.IServiceCollection.
        //     This method should be used only after dependency injection registration phase
        //     completed.
        //internal static T? GetService<T>(this IServiceCollection services)
        //{
        //    return services.GetSingletonInstance<IAbpApplication>().ServiceProvider.GetService<T>();
        //}

        //
        // 摘要:
        //     Resolves a dependency using given Microsoft.Extensions.DependencyInjection.IServiceCollection.
        //     This method should be used only after dependency injection registration phase
        //     completed.
        //internal static object? GetService(this IServiceCollection services, Type type)
        //{
        //    return services.GetSingletonInstance<IAbpApplication>().ServiceProvider.GetService(type);
        //}

        //
        // 摘要:
        //     Resolves a dependency using given Microsoft.Extensions.DependencyInjection.IServiceCollection.
        //     Throws exception if service is not registered. This method should be used only
        //     after dependency injection registration phase completed.
        //public static T GetRequiredService<T>(this IServiceCollection services) where T : notnull
        //{
        //    return services.GetSingletonInstance<IAbpApplication>().ServiceProvider.GetRequiredService<T>();
        //}

        //
        // 摘要:
        //     Resolves a dependency using given Microsoft.Extensions.DependencyInjection.IServiceCollection.
        //     Throws exception if service is not registered. This method should be used only
        //     after dependency injection registration phase completed.
        //public static object GetRequiredService(this IServiceCollection services, Type type)
        //{
        //    return services.GetSingletonInstance<IAbpApplication>().ServiceProvider.GetRequiredService(type);
        //}

        //
        // 摘要:
        //     Returns a System.Lazy`1 to resolve a service from given Microsoft.Extensions.DependencyInjection.IServiceCollection
        //     once dependency injection registration phase completed.
        //public static Lazy<T?> GetServiceLazy<T>(this IServiceCollection services)
        //{
        //    return new Lazy<T>(services.GetService<T>, isThreadSafe: true);
        //}

        //
        // 摘要:
        //     Returns a System.Lazy`1 to resolve a service from given Microsoft.Extensions.DependencyInjection.IServiceCollection
        //     once dependency injection registration phase completed.
        //public static Lazy<object?> GetServiceLazy(this IServiceCollection services, Type type)
        //{
        //    IServiceCollection services2 = services;
        //    Type type2 = type;
        //    return new Lazy<object>(() => services2.GetService(type2), isThreadSafe: true);
        //}

        //
        // 摘要:
        //     Returns a System.Lazy`1 to resolve a service from given Microsoft.Extensions.DependencyInjection.IServiceCollection
        //     once dependency injection registration phase completed.
        //public static Lazy<T> GetRequiredServiceLazy<T>(this IServiceCollection services) where T : notnull
        //{
        //    return new Lazy<T>(services.GetRequiredService<T>, isThreadSafe: true);
        //}

        //
        // 摘要:
        //     Returns a System.Lazy`1 to resolve a service from given Microsoft.Extensions.DependencyInjection.IServiceCollection
        //     once dependency injection registration phase completed.
        //public static Lazy<object> GetRequiredServiceLazy(this IServiceCollection services, Type type)
        //{
        //    IServiceCollection services2 = services;
        //    Type type2 = type;
        //    return new Lazy<object>(() => services2.GetRequiredService(type2), isThreadSafe: true);
        //}

        //public static IServiceProvider? GetServiceProviderOrNull(this IServiceCollection services)
        //{
        //    return services.GetObjectOrNull<IServiceProvider>();
        //}
    }
}
