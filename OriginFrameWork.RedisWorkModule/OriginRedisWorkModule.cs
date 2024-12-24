using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.Common.RedisHelper;
using OriginFrameWork.CoreModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.RedisWorkModule
{
    public class OriginRedisWorkModule:OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            context.Services.AddSingleton<RedisCore>();
            context.Services.AddTransient<IRedisWorker, RedisWorker>();
            base.ConfigureServices(context);
        }
    }
}
