using Microsoft.Extensions.Configuration;
using OriginFrameWork.Core.Configuration;
using StackExchange.Redis;

namespace OriginFrameWork.Common.RedisHelper;
/// <summary>
/// redis核心类
/// </summary>
/// 
public class RedisCore
{
    public IConfiguration Configuration { get; }
    public ConnectionMultiplexer Conn { get; set; }
    public IDatabase Db { get; set; }
    public RedisCore(IConfiguration configuration)
    {
        Configuration = configuration;
        //获取redis连接字符串
        var redisConnStr = Configuration.GetSection("Connection").Get<RedisConnection>().RedisConnectionString;
        //将redis连接字符串转换为ConfigurationOptions对象，该对象包含了redis连接相关的各种配置选项
        ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(redisConnStr);
        configurationOptions.DefaultDatabase = Configuration.GetSection("Connection").Get<RedisConnection>().RedisDb; ;
        //创建redis连接对象
        Conn = ConnectionMultiplexer.Connect(configurationOptions);
        //开启管理员权限
        configurationOptions.AllowAdmin = true;
        //从连接对象中获取redis数据库对象
        Db = Conn.GetDatabase();
    }
}
