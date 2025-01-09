using OriginFrameWork.CoreModule;
using OriginFrameWork.EntityFrameWorkCoreModule;

namespace OriginFrameWork.EntityFrameWork
{
    [OriginInject(typeof(OriginFrameWorkEntityFrameWorkCoreModule))]
    public class OriginFrameWorkEntityFrameWorkModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
