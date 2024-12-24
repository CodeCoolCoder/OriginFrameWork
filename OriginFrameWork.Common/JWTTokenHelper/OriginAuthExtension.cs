using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OriginFrameWork.Core.Configuration;



namespace OriginFrameWork.Common.JWTTokenHelper;

public static class OriginAuthExtension
{
    /// <summary>
    /// 鉴权注册
    /// </summary>
    /// <param name="services"></param>
    public static void AuthenticationRegister(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var config = provider.GetService<IConfiguration>();
        //鉴权模块
        //首先从配置文件中获取部分配置到我们建立的jwt模型中JWTTokenModel
        var token = config.GetSection("Jwt").Get<JWTTokenModel>();
        //注入我们的鉴权模块,括号里表示使用的是默认的jwt鉴权策略，在下面还要注入鉴权模块
        //bearer是一种微软集成的默认鉴权策略
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
         //AddJwtBearer使用默认方案 AuthenticationScheme启用 JWT 持有者身份验证。
         opt =>
         {
             //表示请求是否必须是https，开发环境使用false，正式环境建议true
             opt.RequireHttpsMetadata = false;
             //是否储存我们的token
             //定义在成功授权后是否应将持有者令牌存储在持有者令牌中 AuthenticationProperties 。
             opt.SaveToken = true;
             //TokenValidationParameters获取或设置用于验证标识令牌的参数。
             opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
             {
                 //颁发者签名密钥
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Security)),
                 //有效颁发者
                 ValidIssuer = token.Issuer,
                 //有效接收者
                 ValidAudience = token.Audience
                
             };
             //Events应用程序提供的对象，用于处理持有者身份验证处理程序引发的事件。
             //应用程序可以完全实现接口，也可以创建 JwtBearerEvents 实例，并仅将委托分配给它要处理的事件。
             opt.Events = new JwtBearerEvents()
             {
                 //OnChallenge在质询发送回调用方之前调用。鉴权失败时调用
                 OnChallenge = context =>
                 {
                     //context当 PropertiesContext<TOptions> 对使用 JWT 持有者进行身份验证的资源的访问受到质询时。
                     //HandleResponse鉴权失败，停止代码，此时程序停止了。
                     context.HandleResponse();
                     var res = "{\"code\":401,\"err\":\"无权限\"}";
                     //加上这句话，上面的返回值就是json格式，不加则是返回一串字符串
                     context.Response.ContentType = "application/json";
                     //返回的错误代码401
                     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                     //WriteAsync将给定文本写入响应正文。 将使用 UTF-8 编码。
                     context.Response.WriteAsync(res);
                     //FromResult创建指定结果的、成功完成的 Task<TResult>。
                     return Task.FromResult(0);
                 }
             };
         }
         );
    }
    /// <summary>
    /// 授权注册
    /// </summary>
    /// <param name="services"></param>
    public static void AuthorizationRegister(this IServiceCollection services)
    {
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
    }
}
