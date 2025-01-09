namespace OriginFrameWork.CoreModule.OriginConfigCore;
/// <summary>
/// 配置文件实体类
/// </summary>
public class Connection
{
    public string DbType { get; set; }
    public string DbConnectionString { get; set; }
    public string RedisConnectionString { get; set; }
    public string UseRedis { get; set; }
    public int RedisDb { get; set; }
}