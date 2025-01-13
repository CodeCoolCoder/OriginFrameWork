namespace OriginFrameWork.CoreModule
{
    /// <summary>
    /// 所有扩展模块需继承该类
    /// </summary>
    public abstract class OriginModule : IOriginModule
    {
        protected internal OriginServiceConfigurationContext ServiceConfigurationContext
        {
            get
            {
                if (_serviceConfigurationContext == null)
                {
                    throw new Exception($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}");
                }

                return _serviceConfigurationContext;
            }
            internal set => _serviceConfigurationContext = value;
        }
        private OriginServiceConfigurationContext? _serviceConfigurationContext;

        protected internal OriginApplicationInitializationContext ApplicationInitializationContext
        {
            get
            {
                if (_applicationInitializationContext == null)
                {
                    throw new Exception($"{nameof(ApplicationInitializationContext)} is only available in the {nameof(ConfigureServices)}");
                }

                return _applicationInitializationContext;
            }
            internal set => _applicationInitializationContext = value;
        }
        private OriginApplicationInitializationContext? _applicationInitializationContext;

        public virtual void ConfigureServices(OriginServiceConfigurationContext context)
        {

        }

        public virtual void ApplicationInitialization(OriginApplicationInitializationContext context)
        {

        }
    }
}
