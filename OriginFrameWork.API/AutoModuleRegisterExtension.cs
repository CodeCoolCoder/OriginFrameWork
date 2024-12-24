using Autofac;
using Microsoft.Extensions.DependencyModel;
using OriginFrameWork.CoreModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.Core.Extensions
{
    public class AutoModuleRegisterExtension : Autofac.Module
    {
        public AutoModuleRegisterExtension(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        //protected override void Load(ContainerBuilder builder)
        //{
        //    //nuget包获取
        //    var assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies();
        //    foreach (var assemblyName in assemblyNames)
        //    {
        //        var assembly = Assembly.Load(assemblyName);
        //        var types = assembly.GetTypes().Where(t => t.GetCustomAttribute<OriginInject>() != null);
        //        foreach (var type in types)
        //        {
        //            var attribute = type.GetCustomAttribute<OriginInject>();
        //            if (attribute.ModuleType.IsInterface)
        //            {
        //                builder.RegisterType(type).As(attribute.ModuleType);
        //                var res = attribute.ModuleType;
        //                var context = new OriginServiceConfigurationContext(Services);
        //                var ress = Activator.CreateInstance(res) as IOriginModule;
        //               // ress.ConfigureServices();
        //            }
        //            else
        //            {
        //                builder.RegisterType(type);
        //            }
        //        }
        //        //文件夹
        //        //AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
        //        //foreach (var referencedAssembly in referencedAssemblies)
        //        //{
        //        //    var types2 = Assembly.Load(referencedAssembly);
        //        //    var ressss = types2.GetTypes().Where(t => t.GetCustomAttribute<OriginAttribute>() != null);
        //        //    foreach (var type in ressss)
        //        //    {
        //        //        var attribute = type.GetCustomAttribute<OriginAttribute>();
        //        //        builder.RegisterType(type).As(attribute.ModuleType);
        //        //    }
        //        //}
        //    }
        //}
    }
}
