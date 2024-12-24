using System.Reflection;
using StackExchange.Redis;

namespace OriginFrameWork.Common.RedisHelper;
/// <summary>
/// redis操作hash类
/// </summary>
public partial class RedisWorker
{
    /// <summary>
    /// 存储单纯的键值对
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entries"></param>
    public void SetHashMemory(string key, params HashEntry[] entries)
    {
        RedisCore.Db.HashSet(key, entries);
    }
    /// <summary>
    /// 将字典转换为hashentry存入redis
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dics"></param>
    public void SetHashMemory(string key, Dictionary<string, string> dics)
    {
        var hashEntry = new List<HashEntry>();
        foreach (var dic in dics)
        {
            hashEntry.Add(new HashEntry(dic.Key, dic.Value));
        }
        SetHashMemory(key, hashEntry.ToArray());
    }

    /// <summary>
    /// 将实体转换为hashentry存入redis
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>
    public void SetHashMemory<T>(string key, T entity, Type type = null)
    {
        // 如果type为null，则将其设置为T的类型
        type ??= typeof(T);
        List<HashEntry> hashEntries = new();
        PropertyInfo[] props = type.GetProperties();
        foreach (var prop in props)
        {
            string name = prop.Name;
            object value = prop.GetValue(entity);
            //如果值为null就设为空
            if (value == null)
            {
                value = "";
            }
            //redis无法存储bool类型的值，需转换为0,1形式
            if (value.GetType().Name == "Boolean") value = (bool)value ? 1 : 0;
            {
                hashEntries.Add(new HashEntry(name, value.ToString()));
            }
        }
        SetHashMemory(key, hashEntries.ToArray());
    }

    public void SetHashMemory<T>(string key, IEnumerable<T> entities, Func<T, IEnumerable<string>> func)
    {
        Type type = typeof(T);
        foreach (var entity in entities)
        {
            var valueKeys = func(entity);
            SetHashMemory($"{key}:{string.Join(":", valueKeys)}", entity, type);
        }
    }
    /// <summary>
    /// 获取hash
    /// </summary>
    /// <param name="keyLike"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> GetHashMemory<T>(string keyLike) where T : new()
    {
        var keys = GetKeys(keyLike);
        List<T> ts = new();
        foreach (var key in keys)
        {
            T t = new();
            // 这里拿到的其实是一个集合
            // 我们要循环这个集合，拿到里面的每一个kv
            var res = RedisCore.Db.HashGetAll(key);
            var props = t.GetType().GetProperties();
            foreach (var item in res)
            {
                foreach (var prop in props)
                {
                    if (prop.Name == item.Name)
                    {
                        var nullt = prop.PropertyType;
                        /// <summary>
                        /// 获取nullable<t>中的t类型，如果不为nullable类型则返回null
                        /// </summary>
                        /// <returns></returns>
                        var nulltype = Nullable.GetUnderlyingType(nullt);
                        if (nulltype != null)
                        {
                            nullt = nulltype;
                        }
                        prop.SetValue(t, Convert.ChangeType(item.Value, nullt));
                        break;
                    }
                }
            }
            ts.Add(t);
        }
        return ts;
    }
}
