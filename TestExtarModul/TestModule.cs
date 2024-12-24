using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExtarModul
{
    public class TestModule:OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            context.Services.AddTransient<IExtramodul, Extramodul>();
            base.ConfigureServices(context);
        }
    }
}
