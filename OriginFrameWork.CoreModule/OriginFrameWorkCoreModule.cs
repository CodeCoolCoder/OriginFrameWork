using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using OriginFrameWork.CoreModule.Extensions;
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
        public override void ApplicationInitialization(OriginApplicationInitializationContext context)
        {
            //缺少调用的方法
            base.ApplicationInitialization(context);
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("any");
            app.UseHttpsRedirection();
            app.UseAuthorization();
        }
    }
}
