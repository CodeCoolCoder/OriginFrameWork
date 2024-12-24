using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.ScheduledTaskModule
{
    public class OriginScheduledTaskModule:OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            var service = context.Services;
            service.AddTransient<IOriginScheduledTaskHandler,OriginScheduledTaskHandler>();
            base.ConfigureServices(context);
        }
    }
}
