
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
builder.Services.OriginModuleRegister();
//授权配置注册
builder.Services.AuthorizationRegister();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
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
