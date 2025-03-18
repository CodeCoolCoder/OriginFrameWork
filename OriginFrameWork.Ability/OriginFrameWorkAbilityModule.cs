using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.Ability;
using OriginFrameWork.ConsulModule;
using OriginFrameWork.CoreModule;
using OriginFrameWork.EntityFrameWork;
using OriginFrameWork.JwtBearerModule;
using OriginFrameWork.RemoteInvokeModule;

namespace OriginFrameWork.Core
{
    /// <summary>
    /// 能力层，用于定义各种能力模块
    /// </summary>
    [OriginInject(typeof(OriginFrameWorkEntityFrameWorkModule),
        typeof(OriginRemoteInvokeModule),
        typeof(OriginConsulModule), typeof(OriginFrameWorkJwtBearerModule))]
    public class OriginFrameWorkAbilityModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            //automapper注册
            services.AddAutoMapper(typeof(OriginSystemProfile));


        }
        public override void ApplicationInitialization(OriginApplicationInitializationContext context)
        {
            base.ApplicationInitialization(context);


        }


    }
}
