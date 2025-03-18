
using Castle.DynamicProxy;
using OriginFrameWork.RemoteInvokeModule.RemoteAttributes;
using OriginFrameWork.RemoteInvokeModule.RemoteServerTodo;
using System.Reflection;
using System.Text.Json;


namespace OriginFrameWork.RemoteInvokeModule.DynamicProxy
{
    /// <summary>
    /// 远程服务调用拦截器
    /// 负责拦截接口方法调用并转换为远程调用
    /// </summary>
    public class RemoteServiceInterceptor : IInterceptor
    {
        private readonly IRemoteServiceInvoker _serviceInvoker;
        public RemoteServiceInterceptor(IRemoteServiceInvoker serviceInvoker)
        {
            _serviceInvoker = serviceInvoker;
        }
        /// <summary>
        /// 拦截方法调用
        /// </summary>
        /// <param name="invocation">方法调用信息</param>
        public void Intercept(IInvocation invocation)
        {

            // 获取方法上的特性
            var serviceMethodAttr = invocation.Method.GetCustomAttribute<RemoteServiceIndividualAttribute>();
            // 获取接口上的特性
            var serviceClassAttr = invocation.Method.DeclaringType.GetCustomAttribute<RemoteServiceAttribute>();
            var finnalhttpMethod = serviceClassAttr.remoteHttpMethod;
            if (serviceMethodAttr != null)
            {
                finnalhttpMethod = serviceMethodAttr.HttpMethodByMethod;
            }
            if (serviceClassAttr == null)
            {
                throw new InvalidOperationException($"Service {invocation.Method.DeclaringType.FullName} is not marked with RemoteServiceAttribute");
            }
            //携带的参数
            var parameterInfos = invocation.Method.GetParameters().ToList();
            Dictionary<string, object> parameterGetDicData = new Dictionary<string, object>();
            Dictionary<string, Type> parameterGerDicType = new Dictionary<string, Type>();
            Dictionary<Type, string> postData = new Dictionary<Type, string>();

            if (finnalhttpMethod.ToUpper() == "GET")
            {
                //参数存入字典
                for (var i = 0; i < parameterInfos.Count; i++)
                {
                    parameterGetDicData.Add(parameterInfos[i].Name, invocation.Arguments[i]);
                    parameterGerDicType.Add(parameterInfos[i].Name, parameterInfos[i].ParameterType);
                }
            }
            else
            {
                for (var i = 0; i < parameterInfos.Count; i++)
                {

                    var obj = invocation.Arguments[i];
                    var objtype = obj.GetType();
                    var json = JsonSerializer.Serialize(obj);
                    postData.Add(objtype, json);
                    parameterGetDicData.Add(parameterInfos[i].Name, invocation.Arguments[i]);
                    parameterGerDicType.Add(parameterInfos[i].Name, parameterInfos[i].ParameterType);
                }
            }


            // 创建调用上下文
            var context = new RemoteServiceInvocationContext(
                serviceClassAttr.RemoteServiceGroup,
                invocation.Method.Name,
                parameterGetDicData,
                parameterGerDicType,
                invocation.Method.ReturnType,
                invocation.Method,
                finnalhttpMethod,
                postData);

            // 处理异步方法调用
            var resultType = invocation.Method.ReturnType;
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                resultType = resultType.GetGenericArguments()[0];
                var result = typeof(IRemoteServiceInvoker)
                    .GetMethod(nameof(IRemoteServiceInvoker.InvokeAsync))
                    .MakeGenericMethod(resultType)
                    .Invoke(_serviceInvoker, new object[] { context });

                invocation.ReturnValue = result;
            }
            else
            {
                throw new NotSupportedException("Only async methods are supported");
            }
        }
    }
}