using Microsoft.OpenApi.Models;
using OriginFrameWork.ConsulModule;
using OriginFrameWork.Core;
using OriginFrameWork.CoreModule;

namespace OriginFrameWork.API
{
    /// <summary>
    /// OriginFrameWorkCoreModule为了注册业务服务iservie，需继承ioc的或iocgeneric
    /// </summary>
    [OriginInject(typeof(OriginFrameWorkCoreModule),
     typeof(OriginFrameWorkAbilityModule), typeof(OriginConsulModule))]
    public class OriginFrameWorkAPIModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSwaggerGen(c =>
            {
                // 添加安全定义
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "格式：Bearer {token}",
                    Name = "Authorization", // 默认的参数名
                    In = ParameterLocation.Header,// 放于请求头中
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                // 添加安全要求
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                   {
                        new OpenApiSecurityScheme{
                             Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, new string[]{}
                   }
                });
            });
            base.ConfigureServices(context);
        }
    }
}
