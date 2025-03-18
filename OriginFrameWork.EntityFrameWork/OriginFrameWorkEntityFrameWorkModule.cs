using OriginFrameWork.CoreModule;
using OriginFrameWork.EntityFrameWorkCoreModule;
using OriginFrameWork.EntityFrameWorkCoreModule.Microsoft.Extensions.DependencyInjection;

namespace OriginFrameWork.EntityFrameWork
{
    [OriginInject(typeof(OriginFrameWorkEntityFrameWorkCoreModule))]
    public class OriginFrameWorkEntityFrameWorkModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            context.Services.AddOriginDbContext<OriginDbContext>(m =>
            {
                m.ConfigurationPath = "D:\\MyCode\\OriginFrameWork1.0.3\\OriginFrameWork.API\\appsettings.json";
            });
            context.Services.AddBaseRepository();
        }
    }
}
