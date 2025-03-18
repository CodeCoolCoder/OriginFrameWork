
using OriginFrameWork.API;

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
//builder.Services.AuthenticationRegister();

builder.Services.OriginModuleConfigRegister();
//builder.Services.ServiceRegister();
//授权配置注册
//builder.Services.AuthorizationRegister();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//更改配置及相关操作
builder.Services.useoption1(opt =>
{
    opt.name = "123";
});
builder.Services.tconfigure<testoption>(opt =>
{
    opt.name = "456";
});
// builder.Services.AddSwaggerGen();
//配置跨域
builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();
app.OriginModuleApplicationRegister();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseCors("any");
//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
