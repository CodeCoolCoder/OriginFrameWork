using System.Reflection;

namespace OriginFrameWork.RemoteInvokeModule
{
    /// <summary>
    /// 远程服务调用上下文
    /// 包含调用远程服务所需的所有信息
    /// </summary>
    public class RemoteServiceInvocationContext
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public string RequestType { get; set; }



        /// <summary>
        /// 方法参数
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }
        public Dictionary<string, Type> TypeParameters { get; set; }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public Type ReturnType { get; set; }
        public Dictionary<Type, string> PostData { get; set; }

        /// <summary>
        /// 方法信息
        /// </summary>
        public MethodInfo Method { get; set; }

        public RemoteServiceInvocationContext(
            string serviceName,
            string methodName,
            Dictionary<string, object> parameters,
            Dictionary<string, Type> typeParameters,
            Type returnType,
            MethodInfo method,
            string requestType,
            Dictionary<Type, string> postData
            )
        {
            ServiceName = serviceName;
            MethodName = methodName;
            Parameters = parameters;
            ReturnType = returnType;
            Method = method;
            RequestType = requestType;
            TypeParameters = typeParameters;
            PostData = postData;
        }
    }
}