using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.CoreModule
{
    /// <summary>
    /// 所有扩展模块需继承该类
    /// </summary>
    public abstract class OriginModule: IOriginModule
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
        public virtual void ConfigureServices(OriginServiceConfigurationContext context)
        {

        }
    }
}
