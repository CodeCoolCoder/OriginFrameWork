


using Castle.DynamicProxy;
using OriginFrameWork.RemoteInvokeModule.RemoteServerTodo;

namespace OriginFrameWork.RemoteInvokeModule.DynamicProxy
{
    /// <summary>
    /// 远程服务代理生成器
    /// 负责为远程服务接口创建动态代理实例
    /// </summary>
    public class RemoteServiceProxyGenerator
    {
        private readonly IRemoteServiceInvoker _serviceInvoker;
        private readonly ProxyGenerator _proxyGenerator;

        public RemoteServiceProxyGenerator(IRemoteServiceInvoker serviceInvoker)
        {
            _serviceInvoker = serviceInvoker;
            _proxyGenerator = new ProxyGenerator();
        }

        /// <summary>
        /// 创建服务接口的代理实例
        /// </summary>
        /// <typeparam name="TService">服务接口类型</typeparam>
        /// <returns>服务接口的代理实例</returns>
        public TService CreateProxy<TService>() where TService : class
        {
            return _proxyGenerator.CreateInterfaceProxyWithoutTarget<TService>(new RemoteServiceInterceptor(_serviceInvoker));
        }
    }
}