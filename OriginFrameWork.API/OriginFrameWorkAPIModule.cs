using OriginFrameWork.Core;
using OriginFrameWork.CoreModule;

namespace OriginFrameWork.API
{
    /// <summary>
    /// OriginFrameWorkCoreModule为了注册业务服务iservie，需继承ioc的或iocgeneric
    /// </summary>
    [OriginInject(typeof(OriginFrameWorkCoreModule), typeof(OriginFrameWorkAbilityModule))]
    public class OriginFrameWorkAPIModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            var services = context.Services;

            base.ConfigureServices(context);
        }
    }
}
