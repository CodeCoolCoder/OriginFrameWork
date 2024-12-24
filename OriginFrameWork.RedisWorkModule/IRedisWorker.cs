
using StackExchange.Redis;

namespace OriginFrameWork.Common.RedisHelper;

public interface IRedisWorker 
{

    List<T> GetHashMemory<T>(string keyLike) where T : new();
    List<string> GetKeys(string key);
    string GetString(string key);
    Task<string> GetStringAsync(string key);
    void RemoveKey(string key);
    void SetHashMemory(string key, params HashEntry[] entries);
    void SetHashMemory(string key, Dictionary<string, string> dics);
    void SetHashMemory<T>(string key, T entity, Type type = null);
    void SetHashMemory<T>(string key, IEnumerable<T> entities, Func<T, IEnumerable<string>> func);
    void SetString(string key, string value, TimeSpan timeSpan);
    Task SetStringAsync(string key, string value, TimeSpan timeSpan);
}
