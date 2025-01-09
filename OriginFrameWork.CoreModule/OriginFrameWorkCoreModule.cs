using OriginFrameWork.CoreModule.OriginServiceRegisterCore;

namespace OriginFrameWork.CoreModule
{
    public class OriginFrameWorkCoreModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;

            services.ServiceRegister();
        }
    }
}
