using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.ConsulModule.Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule;
namespace OriginFrameWork.ConsulModule
{
    public class OriginConsulModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var service = context.Services;
            service.AddConsulServer().AddConsulAnalyse();
        }
        public override void ApplicationInitialization(OriginApplicationInitializationContext context)
        {
            base.ApplicationInitialization(context);
            var app = context.App;
            app.UseConsul(app.Services);
            app.AddCheckHealth();
        }
    }
}
