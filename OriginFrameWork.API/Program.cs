
using OriginFrameWork.Common.JWTTokenHelper;

using OriginFrameWork.CoreModule.OriginServiceRegisterCore;


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

builder.Services.OriginModuleRegister();
//初始化配置文件，将配置文件中的值映射到配置实体类中

//builder.Services.AddTransient(typeof(IOriginUnitOfWork<>), typeof(OriginUnitOfWork<>));
//builder.Services.AddDbContext<OriginFrameWorkDbContext>();

//授权配置注册
builder.Services.AuthorizationRegister();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//启用nlog配置
//builder.Services.AddLogging(loggingBuilder =>
//        {
//            loggingBuilder.ClearProviders();
//            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
//            loggingBuilder.AddNLog();
//        });
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
