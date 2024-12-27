集成automapper，jwttoken，authorizationfilter过滤器，redis操作类，仓储服务层（基于EFC），业务服务层
一、配置文件
在API层的appsetting.json中，配置数据库连接、redis连接、jwt配置、需注册的服务程序集名称IocList等，在代码中详细说明各配置作用

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  ///服务启动时向容器内注入的服务，程序集改名或新增在此作配置即可。
  "IocList": "OriginFrameWork.Service,OriginFrameWork.Common",
  "AllowedHosts": "*",
  ///数据库及redis连接字符串配置
  "Connection": {
    "DbType": "Oracle",
    "DbConnectionString": "数据库连接字符串",
    "RedisConnectionString": "127.0.0.1,Password=123456",
    "UseRedis": "false"
  },
  ///jwt相关配置
  "Jwt": {
    "Issuer": "JG013471",
    "Audience": "JG013471",
	///过期时间（秒）
    "Expires": 43200,
    "Security": "ASDGGFDHFGJBDFDBDHHJGFDHNFG"
  }
}
```

二、Automapper集成使用
项目已集成automapper，使用前需先在OriginSystemProfile中配置映射规则，使用详情见代码说明

```csharp
///配置文件中配置映射规则
public class OriginSystemProfile : Profile
{
    public OriginSystemProfile()
    {
        CreateMap<MoveStockRecord, MoveStockRecordDto>().ReverseMap();
    }
}
//在需要使用的业务服务中注入automapper服务
public class OriginService : IOriginService
{
    public OriginService(IMapper mapper)
    {
        Mapper = mapper;
    }
    public IMapper Mapper { get; }

    public void Test()
    {
        var Tests = Mapper.Map<Tests>(TestsDto);
    }
}
```

三、仓储服务层使用
已封装好对数据库的增删改查操作的仓储服务层，在业务服务层注入使用即可，详情见代码说明

```csharp
public class OriginService : IOriginService
{
	///此处的T为你要操作的实体对应数据库里要增删改查操作的表
    public OriginService(IBaseRepository<T> baseRepository)
    {
        BaseRepository = baseRepository;
    }

    public IBaseRepository<T> BaseRepository { get; }

    public void Test(int id)
    {
        var TModel = BaseRepository.OriginGet(m => m.id == id);
    }
}
```

四、业务服务层使用
通过接口来约束业务服务，一般一个接口层，对应一个实现层，每个接口层需继承IocTag,才会在程序启动时被注册，后续才能使用

```csharp
public interface IOriginService : IocTag
{
	 void Test();
}
public class OriginService : IOriginService
{
	public void Test(){
		string t="1";
	}
}
///在controller层中使用,构造函数注入
public class OriginController : ControllerBase
{
    public OriginController(IOriginService originService)
    {
        OriginService = originService;
    }
    public IOriginService OriginService { get; }
    [HttpPost("/OriginService")]
    public void Origin()
    {
 		string str= OriginService.Test();
    }
}
```

五、CodeFirst生成数据表
使用EntityFrameWork的Codefirst，通过实体来映射数据库自动生成相应的表结构，此处以mysql为例
使用终端，切换至Core下面，执行dotnet ef migrations add init1
在执行dotnet ef database update
后续还要对实体进行更改，同时希望应用到数据库中的，重复执行上面两步（init1为定义配置文件名，需要更改）
同时，需要映射到数据库的实体，必须继承BaseModel，才会向数据库映射，一些业务实体，无需映射的则不继承BaseModel即可。

```csharp
public class Test : BaseModel
{

}
```

六、jwttoken使用
框架已集成jwttoken，使用时只需在需要进行授权的接口加上特性即可[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

```csharp
[ApiController]
[Route("api/[controller]")]
//加上特性，所有的请求必须含token，否则无数据，也可加在单个请求上
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OriginController : ControllerBase
{
	///若想某个请求不做授权控制，加上AllowAnonymous即可
    [HttpPost("/OriginService")，AllowAnonymous]
    public void Origin()
    {

    }
}
```

若想个性化jwttoken的配置设置，在JWTTokenModel中更改配置，同时在tokenhelper中也需做相应更改

```csharp
public class JWTTokenModel
{
    /// <summary>
    /// 发行人
    /// </summary>
    /// <value></value>
    public string Issuer { get; set; }
    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public string Audience { get; set; }
    /// <summary>
    /// 过期时间
    /// </summary>
    /// <value></value>
    public int Expires { get; set; }

    public string Security { get; set; }
	//以下配置为个性化配置
    public int Id { get; set; }
    public string UserNo { get; set; }
    public string UserName { get; set; }
}
```

```csharp
 public static string CreateToken(JWTTokenModel jWTToken)
    {
        /// <summary>
        /// claims相当于一张入场卷，每一个claim相当于入场卷内的一条信息，所有信息组合起来决定你是否可以入场，此处的claim的信息，需和JWTTokenModel中的信息对上
        /// </summary>
        /// <value></value>
        var claims = new[] {
            new Claim("Id",jWTToken.Id.ToString()) ,
            new Claim("UserNo",jWTToken.UserNo) ,
            new Claim("UserName",jWTToken.UserName) ,
        };
        //利用SymmetricSecurityKey生成密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTToken.Security));
        /// <summary>
        /// 表示用于生成数字签名的加密密钥和安全算法。
        /// </summary>
        /// <returns>SecurityKey key--包含用于生成数字签名的加密密钥的安全密钥。</returns>
        /// <returns>SecurityAlgorithms 一个 URI，表示用于生成数字签名的加密算法。</returns>
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //获得token的实例
        var token = new JwtSecurityToken(
            issuer: jWTToken.Issuer,
            audience: jWTToken.Audience,
            expires: DateTime.Now.AddMinutes(jWTToken.Expires),
            signingCredentials: creds,
            claims: claims);
        //将此实例处理的类型标记序列化为 XML。
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return accessToken;
    }
```

七、Redis使用
框架已集成redis，在服务启动时已注入redis操作类，只需在使用的地方注入即可，详情见代码示例。

```csharp
public class OriginService : IOriginService
{
    public OriginService(IRedisWorker redisWorker)
    {
        RedisWorker = redisWorker;
    }

    public IRedisWorker RedisWorker { get; }

    public void Test()
    {
        RedisWorker.SetString("test", "test", TimeSpan.FromMicroseconds(1));
    }
}
```
