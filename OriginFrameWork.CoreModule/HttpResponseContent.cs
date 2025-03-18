namespace OriginFrameWork.HttpResponseContentModule;

/// <summary>
/// 统一返回结果
/// </summary>
public class HttpResponseContent
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// 数据
    /// </summary>
    public object Data { get; private set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; private set; }

    private HttpResponseContent()
    {
        Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 成功
    /// </summary>
    public static HttpResponseContent Ok()
    {
        return new HttpResponseContent
        {
            Success = true,
            Code = ResultCode.Success,
            Message = "操作成功"
        };
    }

    /// <summary>
    /// 成功并返回数据
    /// </summary>
    public static HttpResponseContent Ok(object data)
    {
        return new HttpResponseContent
        {
            Success = true,
            Code = ResultCode.Success,
            Message = "操作成功",
            Data = data
        };
    }

    /// <summary>
    /// 成功并返回消息
    /// </summary>
    public static HttpResponseContent Ok(string message)
    {
        return new HttpResponseContent
        {
            Success = true,
            Code = ResultCode.Success,
            Message = message
        };
    }

    /// <summary>
    /// 成功并返回数据和消息
    /// </summary>
    public static HttpResponseContent Ok(object data, string message)
    {
        return new HttpResponseContent
        {
            Success = true,
            Code = ResultCode.Success,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 失败
    /// </summary>
    public static HttpResponseContent Fail()
    {
        return new HttpResponseContent
        {
            Success = false,
            Code = ResultCode.Fail,
            Message = "操作失败"
        };
    }

    /// <summary>
    /// 失败并返回消息
    /// </summary>
    public static HttpResponseContent Fail(string message)
    {
        return new HttpResponseContent
        {
            Success = false,
            Code = ResultCode.Fail,
            Message = message
        };
    }

    /// <summary>
    /// 失败并返回消息和状态码
    /// </summary>
    public static HttpResponseContent Fail(string message, string code)
    {
        return new HttpResponseContent
        {
            Success = false,
            Code = code,
            Message = message
        };
    }

    /// <summary>
    /// 失败并返回数据和消息
    /// </summary>
    public static HttpResponseContent Fail(object data, string message)
    {
        return new HttpResponseContent
        {
            Success = false,
            Code = ResultCode.Fail,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 自定义返回结果
    /// </summary>
    public static HttpResponseContent Custom(bool success, string code, string message, object data = null)
    {
        return new HttpResponseContent
        {
            Success = success,
            Code = code,
            Message = message,
            Data = data
        };
    }
}

/// <summary>
/// 状态码常量
/// </summary>
public static class ResultCode
{
    /// <summary>
    /// 成功
    /// </summary>
    public const string Success = "200";

    /// <summary>
    /// 失败
    /// </summary>
    public const string Fail = "400";

    /// <summary>
    /// 未授权
    /// </summary>
    public const string Unauthorized = "401";

    /// <summary>
    /// 禁止访问
    /// </summary>
    public const string Forbidden = "403";

    /// <summary>
    /// 服务器错误
    /// </summary>
    public const string Error = "500";

    /// <summary>
    /// Token过期
    /// </summary>
    public const string TokenExpired = "461";

    /// <summary>
    /// 业务错误
    /// </summary>
    public const string Business = "600";
}