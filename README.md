## 用户鉴权授权模块引入

1.能力层引入 OriginFrameWorkJwtBearerModule 包

```csharp
    [OriginInject(typeof(OriginFrameWorkJwtBearerModule))]
    public class OriginFrameWorkAbilityModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            //automapper注册
            services.AddAutoMapper(typeof(OriginSystemProfile));
        }
    }
```

2.配置文件内完善相关配置

```json
  "JwtAuth": {
    "Audience": "OriginFrameWork",
    "Issuer": "OriginFrameWork",
    "SecurityKey": "BB3647441FFA4B5DB4E64A29B53CE525",
    "Expires": 48
  }
```

4.token 生成依赖注入

```csharp
 public OriginController(TokenCreateModel tokenCreateModel)
  {
      TokenCreateModel = tokenCreateModel;
  }
 public TokenCreateModel TokenCreateModel { get; }
 //生成token的方法
 TokenCreateModel.GetToken("ss");
```

3.在需要控制的控制器或局部方法添加[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]特性

4.请求携带头部 Authorization:Bearer "your token";

## 2.远程调用模块引入(RPC)

1.服务端客户端均引入 OriginFrameWork.RemoteInvokeModule 包

```csharp
    [OriginInject(typeof(OriginRemoteInvokeModule))]
    public class OriginFrameWorkAbilityModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            //automapper注册
            services.AddAutoMapper(typeof(OriginSystemProfile));
        }
    }
```

2.客户端完善配置文件信息（要请求的服务端地址等信息）

```json
  "RemoteServices": {
    "OriginService": {
      "BaseUrl": "http://localhost:7178",
      "Prefix": "app/api"
    }

  },
```

3.服务端在需要转换为远程服务的接口上添加特性(部分信息需与配置文件一致)

```csharp
[RemoteServiceAttribute("OriginService")]
public interface IOriginService : IRemoteServiceTag
{
    [RemoteServiceIndividualAttribute("POST")]
    Task<string> GetString(string get);
}
```

## 3.EntityFrameWork 层引入

如果我们需要集成 **Entity Framework Core** 只需要引入其模块就行，当然如果你觉得项目需要分层，也可以另外创建一个数据访问层，再集成 **Entity Framework Core** 模块即可。

配置文件信息完善

```json
  "Connection": {
    "DbType": "MySql",
    "DbConnectionString": "xxxxxxxxx",
    "RedisConnectionString": "xxxxxxxx",
    "RedisDb": 0,
    "UseRedis": "true"
  },
```

目前 **Entity Framework Core** 只适配了 **MySQL** 、**Oracle、SqlServer** 模块，后期将适配更多模块

```csharp
    [OriginInject(typeof(OriginFrameWorkEntityFrameWorkCoreModule))]
    public class OriginFrameWorkEntityFrameWorkModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            context.Services.AddOriginDbContext<OriginDbContext>(m =>
            {
		//你的配置文件地址
                m.ConfigurationPath = "D:\\MyCode\\OriginFrameWork1.0.3\\OriginFrameWork.API\\appsettings.json";
            });
            context.Services.AddBaseRepository();
        }
    }
```

```csharp
   public class OriginDbContextFactory : IDesignTimeDbContextFactory<OriginDbContext>
   {
       public OriginDbContext CreateDbContext(string[] args)
       {
           return new OriginDbContext();
       }
   }
```

1.使用 codefirst,需要先新建实体，并且继承 BaseModel

```csharp
public partial class MainDevice : BaseModel
{
    public int Id { get; set; }

    public string? MaterialNo { get; set; }

    public string? MaterialName { get; set; }

}
```

2.新建 dbcontext 并继承 OriginDbContextBase

```csharp
    public class OriginDbContext : OriginDbContextBase
    {
        public OriginDbContext() : base()
        {

        }
        public OriginDbContext(DbContextOptions<OriginDbContext> options)
      : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
        //// 可以选择性地重写以下方法来自定义行为
        //protected override void RegisterEntities(ModelBuilder modelBuilder)
        //{
        //    base.RegisterEntities(modelBuilder);
        //    // 添加自定义实体映射
        //}

        //protected override IEnumerable<Type> GetEntityTypes()
        //{
        //    // 自定义实体类型获取逻辑
        //    return base.GetEntityTypes();
        //}
    }
```

3.将配置文件复制进 EntityFramework 项目层（供 codefirst 使用,还需要安装 EntityFrameworkCore.Design 包）

4.使用命令自动建表并生成上下文(切换命令行目录在 entityframework 这一层中)

```shell
dotnet ef migrations add init
dotnet ef database update 对数据库进行更新，对上面更改的配置更新到数据库中CodeFirst

```

3.注入 repository 即可使用

```csharp
public GetDbTest(IBaseRepository<MainDevice,DeviceManagementSystemContext> baseRepository)
{
    BaseRepository = baseRepository;
}

public IBaseRepository<MainDevice, DeviceManagementSystemContext> BaseRepository { get; }

public List<string> GetMainDevices()
{
    return BaseRepository.FindAsIQueryable().Select(m => m.MaterialName).ToList();
}
```

4.Dbfirst 反过来，先使用命令生成实体

```shell
dotnet ef dbcontext scaffold  "xxxxxxx" Oracle.EntityFrameworkCore -o Models1 (Oracle数据库映射字符串)

dotnet ef dbcontext scaffold "xxxxxxxxxxxxxx" Pomelo.EntityFrameworkCore.MySql   --table QX_2021_08 --use-database-names(MYSQL数据库映射字符串)
```

5.生成的实体继承 BaseModel,随后即可同 codefirst 一样使用
