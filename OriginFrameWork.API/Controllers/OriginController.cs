using Microsoft.AspNetCore.Mvc;
using OriginFrameWork.Service.OriginApp;

namespace OriginFrameWork.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//加上特性，所有的请求必须含token，否则无数据
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

//使用步骤
//1.appsettings.json中修改数据库类型及连接字符串，及是否使用redis等配置,programs中修改使用的库类型，usemysql或者useoracle
//2.OriginFrameWorkDbContext中修改数据库连接字符串
//3.如果项目名称发生变化，ServiceCollectionRegisterExtension中修改反射的文件名称
//4.token的配置文件在appsettings.json中修改,开启授权解开controller上的注释即可 [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//5.如要使用日志，nlog配置文件中修改seq配置地址即可
//6.如要使用automapper，在orisystemprofile中配置映射规则
//7.如果想放开单个接口的授权，  [HttpGet("/ScanCodeDevice"), AllowAnonymous]，在后面加上AllowAnonymous即可
//8.配置要存储的token信息在tokenhelper和jwttokenmodel中修改对应字段
//9.授权过滤器，获取信息在Fileters中的CtmAuthorizationFilterAttribute进行配置，详细使用后续会出文档，可参考在devicesystem中的使用
public class OriginController : ControllerBase
{
    public OriginController(IOriginService originService, IGetDbTest getDbTest)
    {
        OriginService = originService;
        GetDbTest = getDbTest;
    }

    public IOriginService OriginService { get; }
    public IGetDbTest GetDbTest { get; }

    [HttpPost("/GETDBSERVICE")]
    public List<string> GETDB()
    {
        return GetDbTest.GetMainDevices();
    }
}
