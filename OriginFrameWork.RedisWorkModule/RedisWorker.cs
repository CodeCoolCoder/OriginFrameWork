
using StackExchange.Redis;

namespace OriginFrameWork.Common.RedisHelper;


public partial class RedisWorker : IRedisWorker
{
    /// <summary>
    /// 依赖注入redis核心类
    /// </summary>
    /// <param name="redisCore"></param>
    public RedisWorker(RedisCore redisCore)
    {
        RedisCore = redisCore;
    }
    public RedisCore RedisCore { get; }
    /// <summary>
    /// 通过scan获取所有的适配的key
    /// </summary>
    /// <param name="key">可以带通配符的key</param>
    /// <returns></returns>
    public List<string> GetKeys(string key)
    {
        List<string> keyList = new List<string>();
        //获取当前redis连接中的所有节点信息
        var eps = RedisCore.Conn.GetEndPoints();
        var ep = eps[0];
        //通过endpoints获取redis服务
        var redisServer = RedisCore.Conn.GetServer(ep);
        //获取指定键的所有键名
        var keys = redisServer.Keys(0, key).ToList();
        //将键名转为字符串并存储
        keys.ForEach(k =>
        {
            keyList.Add(k.ToString());
        });
        return keyList;
    }
    /// <summary>
    /// 删除指定的key
    /// </summary>
    /// <param name="key"></param>
    public void RemoveKey(string key)
    {
        RedisCore.Db.KeyDelete(key);
    }




}
