
using StackExchange.Redis;

namespace OriginFrameWork.Common.RedisHelper;

public interface IRedisWorker
{
    T GetHashMemory<T>(string key) where T : class, new();

    List<string> GetKeys(string key);
    string GetString(string key);
    Task<string> GetStringAsync(string key);
    void RemoveKey(string key);
    void SetHashMemory(string key, DateTime expireTime, params HashEntry[] entries);
    void SetHashMemory(string key, Dictionary<string, string> dics, DateTime expireTime);
    void SetHashMemory<T>(string key, T entity, DateTime expireTime, Type type = null);
    void SetHashMemory<T>(string key, IEnumerable<T> entities, DateTime expireTime, Func<T, IEnumerable<string>> func);
    void SetString(string key, string value, TimeSpan timeSpan);
    Task SetStringAsync(string key, string value, TimeSpan timeSpan);
}
