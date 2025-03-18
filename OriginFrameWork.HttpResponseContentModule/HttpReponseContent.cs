namespace OriginFrameWork.HttpResponseContentModule
{
    public class HttpReponseContent<T>
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
        public T Data { get; private set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; private set; }

        public HttpReponseContent()
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 成功
        /// </summary>
        public static HttpReponseContent<T> Ok(T data = default, string message = "操作成功")
        {
            return new HttpReponseContent<T>
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
        public static HttpReponseContent<T> Fail(string message = "操作失败", string code = ResultCode.Fail)
        {
            return new HttpReponseContent<T>
            {
                Success = false,
                Code = code,
                Message = message
            };
        }

        /// <summary>
        /// 自定义错误
        /// </summary>
        public static HttpReponseContent<T> Custom(bool success, string code, string message, T data = default)
        {
            return new HttpReponseContent<T>
            {
                Success = success,
                Code = code,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 从另一个Result转换数据类型
        /// </summary>
        public static HttpReponseContent<T> From<TSource>(HttpReponseContent<TSource> source, T data = default)
        {
            return new HttpReponseContent<T>
            {
                Success = source.Success,
                Code = source.Code,
                Message = source.Message,
                Data = data
            };
        }



    }

    /// <summary>
    /// 无数据的结果类型
    /// </summary>
    public class HttpReponseContent : HttpReponseContent<object>
    {
        public static HttpReponseContent Ok(string message = "操作成功")
        {
            return (HttpReponseContent)HttpReponseContent<object>.Ok(null, message);
        }

        public static HttpReponseContent Fail(string message = "操作失败", string code = ResultCode.Fail)
        {
            return (HttpReponseContent)HttpReponseContent<object>.Fail(message, code);
        }

        public static HttpReponseContent Custom(bool success, string code, string message)
        {
            return (HttpReponseContent)HttpReponseContent<object>.Custom(success, code, message);
        }

        public static HttpReponseContent From<TSource>(HttpReponseContent<TSource> source)
        {
            return (HttpReponseContent)HttpReponseContent<object>.From(source);
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

}
