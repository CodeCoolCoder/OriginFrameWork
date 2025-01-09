using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.Ability;
using OriginFrameWork.CoreModule;
using OriginFrameWork.EntityFrameWork;
using OriginFrameWork.RemoteInvokeModule;

namespace OriginFrameWork.Core
{
    /// <summary>
    /// 能力层，用于定义各种能力模块
    /// </summary>
    [OriginInject(typeof(OriginFrameWorkEntityFrameWorkModule), typeof(OriginRemoteInvokeModule))]
    public class OriginFrameWorkAbilityModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            //automapper注册
            services.AddAutoMapper(typeof(OriginSystemProfile));
        }
    }
}
