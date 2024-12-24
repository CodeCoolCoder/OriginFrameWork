using OriginFrameWork.Core.Extensions;
using OriginFrameWork.CoreModule;

namespace OriginFrameWork.API
{

    public class OriginFrameWorkAPIModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            var service = context.Services;
            service.RepositoryRegister();
            service.ServiceRegister();
            //service.OriginModuleRegister();
            base.ConfigureServices(context);
        }
    }
}
