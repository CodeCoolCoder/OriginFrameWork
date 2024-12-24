namespace OriginFrameWork.Common.RedisHelper;


/// <summary>
/// redis字符串操作类
/// </summary>
public partial class RedisWorker
{
    public void SetString(string key, string value, TimeSpan timeSpan)
    {
        //timeSpan表示过期时间
        RedisCore.Db.StringSet(key, value, timeSpan);
    }
    public async Task SetStringAsync(string key, string value, TimeSpan timeSpan)
    {
        //timeSpan表示过期时间
        await RedisCore.Db.StringSetAsync(key, value, timeSpan);
    }


    public string GetString(string key)
    {
        return RedisCore.Db.StringGet(key);
    }


    public async Task<string> GetStringAsync(string key)
    {
        return await RedisCore.Db.StringGetAsync(key);
    }
}
