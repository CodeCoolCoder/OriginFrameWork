using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using OriginFrameWork.Common.JWTTokenHelper;
using OriginFrameWork.Core.Configuration;
using OriginFrameWork.Core.EFDbContext;
using OriginFrameWork.Core.Extensions;
using OriginFrameWork.Core.UnitOfWork;


var builder = WebApplication.CreateBuilder(args);
//var autofacbuilder = builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(op =>
//{
//    //文件夹权限问题，暂时注释
//    //op.RegisterHotPlugin(builder.Services);
//    //需要改造，带参数了
//    op.RegisterModule<AutoModuleRegisterExtension>();
//});

var config = builder.Configuration;
// Add services to the container.
//鉴权配置注册
builder.Services.AuthenticationRegister();
//将automapper注册到系统中，并且添加实体映射类
builder.Services.AddAutoMapper(typeof(OriginSystemProfile));
//初始化配置文件，将配置文件中的值映射到配置实体类中
Appsetting.Init(builder.Services, config);


var dbtype = config.GetSection("Connection").Get<Connection>().DbType;
var dbstr = config.GetSection("Connection").Get<Connection>().DbConnectionString;
builder.Services.AddDbContext<OriginFrameWorkDbContext>(opt =>
{
    if (string.IsNullOrEmpty(dbtype) || string.IsNullOrEmpty(dbstr))
    {
        Console.WriteLine("数据库连接信息配置错误");
        return;
    }
    switch (dbtype)
    {
        case "Oracle":
            opt.UseOracle(dbstr);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            break;
        case "MySql":
            opt.UseMySql(dbstr, MySqlServerVersion.LatestSupportedServerVersion);
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            break;
        default:
            Console.WriteLine($"暂时不支持{dbtype}数据库");
            break;
    }
});
builder.Services.AddTransient(typeof(IOriginUnitOfWork<>), typeof(OriginUnitOfWork<>));
//利用extension中的反射自动获取并注册服务，替换上面的注册服务的方式
//反射很耗性能，最好是在项目启动时使用
//builder.Services.RepositoryRegister();
//builder.Services.ServiceRegister();
builder.Services.OriginModuleRegister();
//授权配置注册
builder.Services.AuthorizationRegister();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//启用nlog配置
builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog();
        });
//配置跨域
builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("any");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
