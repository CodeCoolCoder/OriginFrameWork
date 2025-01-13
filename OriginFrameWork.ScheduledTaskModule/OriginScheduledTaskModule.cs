using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule;

namespace OriginFrameWork.ScheduledTaskModule
{
    public class OriginScheduledTaskModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            var service = context.Services;
            service.AddTransient<IOriginScheduledTaskHandler, OriginScheduledTaskHandler>();
            base.ConfigureServices(context);
        }
    }
}
