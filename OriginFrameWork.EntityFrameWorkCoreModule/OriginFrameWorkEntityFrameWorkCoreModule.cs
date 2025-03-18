using OriginFrameWork.CoreModule;

namespace OriginFrameWork.EntityFrameWorkCoreModule
{
    public class OriginFrameWorkEntityFrameWorkCoreModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            //var config = context.Services.GetConfiguration();
            //context.Services.AddOriginDbContext<OriginDbContext>(config);

        }
    }
}
