using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OriginFrameWork.CoreModule;
using OriginFrameWork.CoreModule.Extensions;
using System.Text;


namespace OriginFrameWork.JwtBearerModule;

public class OriginFrameWorkJwtBearerModule : OriginModule
{



    public override void ConfigureServices(OriginServiceConfigurationContext context)
    {
        var services = context.Services;
        // var config=services.
        services.AddTransient<TokenCreateModel>();
        var configuration = services.GetConfiguration();
        //注册jwt验证
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration.GetValue<string>("JwtAuth:Audience"),
                ValidIssuer = configuration.GetValue<string>("JwtAuth:Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("JwtAuth:SecurityKey"))),
            };

            /*
                由于我们后期所有的接口都遵循返回标准，code表示返回码0为成功，msg为信息，data为数据。
            因此授权失败也要统一此标准，以便前端可以进行统一判断。  
                修改未授权的输出。我们定义授权失败返回{code="401",msg="无登录信息或登录信息已失效，请重新登录"},            
            */
            opt.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {

                    //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                    context.HandleResponse();
                    var payload = "{\"ret\":203,\"err\":\"无登录信息或登录信息已失效，请重新登录。\"}";
                    //自定义返回的数据类型
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status203NonAuthoritative;
                    context.Response.WriteAsync(payload);
                    return Task.FromResult(0);
                }
            };
        });

        //services.AddSwaggerGen(c =>
        //{
        //    // 添加安全定义
        //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        Description = "格式：Bearer {token}",
        //        Name = "Authorization", // 默认的参数名
        //        In = ParameterLocation.Header,// 放于请求头中
        //        Type = SecuritySchemeType.ApiKey,
        //        BearerFormat = "JWT",
        //        Scheme = "Bearer"
        //    });
        //    // 添加安全要求
        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        //           {
        //                new OpenApiSecurityScheme{
        //                     Reference = new OpenApiReference{
        //                    Type = ReferenceType.SecurityScheme,
        //                    Id = "Bearer"
        //                }
        //            }, new string[]{}
        //           }
        //        });
        //});


        base.ConfigureServices(context);
    }
    //public override void OnApplicationInitialization(ApplicationInitializationContext context)
    //{
    //    base.OnApplicationInitialization(context);
    //}
}
