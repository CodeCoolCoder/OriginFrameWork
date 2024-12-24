namespace OriginFrameWork.Core.Configuration;
/// <summary>
/// 配置文件实体类
/// </summary>
public class RedisConnection
{
    public string DbType { get; set; }
    public string DbConnectionString { get; set; }
    public string RedisConnectionString { get; set; }
    public string UseRedis { get; set; }
    public int RedisDb { get; set; }
}