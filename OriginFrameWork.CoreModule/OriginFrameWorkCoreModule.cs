using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OriginFrameWork.CoreModule.OriginServiceRegisterCore;

namespace OriginFrameWork.CoreModule
{
    public class OriginFrameWorkCoreModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            services.AddHttpContextAccessor();
            services.ServiceRegister();
        }
        public override void ApplicationInitialization(OriginApplicationInitializationContext context)
        {
            base.ApplicationInitialization(context);

            var app = context.App;
            var env = context.App.Environment;
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
