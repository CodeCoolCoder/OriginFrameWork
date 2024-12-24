using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.CoreModule
{
    public interface IOriginModule
    {
        void ConfigureServices(OriginServiceConfigurationContext context);
    }
}
