using StackExchange.Redis;
using System.Collections;
using System.Reflection;
using System.Text.Json;

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
    public void SetHashMemory(string key, DateTime expireTime, params HashEntry[] entries)
    {
        RedisCore.Db.HashSet(key, entries);
        RedisCore.Db.KeyExpire(key, expireTime);

    }
    /// <summary>
    /// 将字典转换为hashentry存入redis
    /// </summary>
    /// <param name="key"></param>
    /// <param name="dics"></param>
    public void SetHashMemory(string key, Dictionary<string, string> dics, DateTime expireTime)
    {
        var hashEntry = new List<HashEntry>();
        foreach (var dic in dics)
        {
            hashEntry.Add(new HashEntry(dic.Key, dic.Value));
        }
        SetHashMemory(key, expireTime, hashEntry.ToArray());
    }

    /// <summary>
    /// 将实体转换为hashentry存入redis
    /// </summary>
    /// <param name="key"></param>
    /// <param name="entity"></param>
    /// <param name="type"></param>
    /// <typeparam name="T"></typeparam>

    public void SetHashMemory<T>(string key, T entity, DateTime expireTime, Type type = null)
    {
        type ??= typeof(T);
        List<HashEntry> hashEntries = new();
        PropertyInfo[] props = type.GetProperties();

        foreach (var prop in props)
        {
            string name = prop.Name;
            object value = prop.GetValue(entity);

            // 处理属性值
            string serializedValue = SerializePropertyValue(value);
            hashEntries.Add(new HashEntry(name, serializedValue));
        }
        SetHashMemory(key, expireTime, hashEntries.ToArray());
    }


    public void SetHashMemory<T>(string key, IEnumerable<T> entities, DateTime expireTime, Func<T, IEnumerable<string>> func)
    {
        Type type = typeof(T);
        foreach (var entity in entities)
        {
            var valueKeys = func(entity);
            SetHashMemory($"{key}:{string.Join(":", valueKeys)}", entity, expireTime, type);
        }
    }

    /// <summary>
    /// 从Redis读取数据并反序列化
    /// </summary>
    public T GetHashMemory<T>(string key) where T : class, new()
    {
        var hashFields = RedisCore.Db.HashGetAll(key);
        if (hashFields.Length == 0)
            return null;
        T result = new T();
        Type type = typeof(T);
        var properties = type.GetProperties();

        foreach (var field in hashFields)
        {
            var prop = properties.FirstOrDefault(p => p.Name == field.Name);
            if (prop == null || !prop.CanWrite)
                continue;

            string value = field.Value;
            if (string.IsNullOrEmpty(value))
                continue;
            try
            {
                object deserializedValue = DeserializePropertyValue(value, prop.PropertyType);
                prop.SetValue(result, deserializedValue);
            }
            catch (Exception ex)
            {
                // 处理反序列化错误
                // 可以选择记录日志或者其他处理方式
                Console.WriteLine($"Error deserializing property {prop.Name}: {ex.Message}");
            }
        }

        return result;
    }

    /// <summary>
    /// 反序列化属性值
    /// </summary>
    private object DeserializePropertyValue(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        // 处理简单类型
        if (IsSimpleType(targetType))
        {
            if (targetType == typeof(bool))
                return value == "1";

            if (targetType.IsEnum)
                return Enum.Parse(targetType, value);

            if (targetType == typeof(DateTime))
                return DateTime.Parse(value);

            if (targetType == typeof(TimeSpan))
                return TimeSpan.FromTicks(long.Parse(value));

            return Convert.ChangeType(value, targetType);
        }

        // 处理集合类型
        if (IsCollectionType(targetType))
        {
            var itemType = targetType.IsArray
                ? targetType.GetElementType()
                : targetType.GetGenericArguments()[0];

            var items = JsonSerializer.Deserialize<List<string>>(value);
            if (targetType.IsArray)
            {
                var array = Array.CreateInstance(itemType, items.Count);
                for (int i = 0; i < items.Count; i++)
                {
                    array.SetValue(DeserializePropertyValue(items[i], itemType), i);
                }
                return array;
            }
            else if (targetType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                foreach (var item in items)
                {
                    list.Add(DeserializePropertyValue(item, itemType));
                }
                return list;
            }
        }

        // 处理复杂对象
        return JsonSerializer.Deserialize(value, targetType, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }





    /// <summary>
    /// 序列化属性值
    /// </summary>
    private string SerializePropertyValue(object value)
    {
        if (value == null)
            return "";

        var type = value.GetType();

        // 处理基本类型
        if (IsSimpleType(type))
        {
            // 处理布尔类型
            if (type == typeof(bool))
                return ((bool)value) ? "1" : "0";

            return value.ToString();
        }

        // 处理枚举类型
        if (type.IsEnum)
        {
            return ((int)value).ToString();
        }

        // 处理日期时间类型
        if (type == typeof(DateTime))
        {
            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        // 处理TimeSpan类型
        if (type == typeof(TimeSpan))
        {
            return ((TimeSpan)value).Ticks.ToString();
        }

        // 处理集合类型
        if (IsCollectionType(type))
        {
            if (value is IEnumerable enumerable)
            {
                var items = new List<string>();
                foreach (var item in enumerable)
                {
                    items.Add(SerializePropertyValue(item));
                }
                return JsonSerializer.Serialize(items);
            }
        }

        // 处理复杂对象（包括嵌套的实体类）
        return JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    /// <summary>
    /// 判断是否为简单类型
    /// </summary>
    private bool IsSimpleType(Type type)
    {
        return type.IsPrimitive
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(TimeSpan)
            || type.IsEnum;
    }

    /// <summary>
    /// 判断是否为集合类型
    /// </summary>
    private bool IsCollectionType(Type type)
    {
        return type.IsArray
            || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>))
            || typeof(IEnumerable).IsAssignableFrom(type);
    }



}
