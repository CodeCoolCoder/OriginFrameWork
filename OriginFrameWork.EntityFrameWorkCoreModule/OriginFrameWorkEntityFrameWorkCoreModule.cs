using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.CoreModule;
using OriginFrameWork.CoreModule.Extensions;
using OriginFrameWork.CoreModule.OriginConfigCore;
using OriginFrameWork.CoreModule.OriginServiceRegisterCore;

namespace OriginFrameWork.EntityFrameWorkCoreModule
{
    public class OriginFrameWorkEntityFrameWorkCoreModule : OriginModule
    {
        public override void ConfigureServices(OriginServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            var services = context.Services;
            var config = services.GetConfiguration();
            //初始化数据库相关配置
            Appsetting.Init(services, config);
            var dbtype = Appsetting._connection.DbType;
            var dbstr = Appsetting._connection.DbConnectionString;
            services.AddDbContext<OriginDbContext>(opt =>
            {
                if (string.IsNullOrEmpty(dbtype) || string.IsNullOrEmpty(dbstr))
                {
                    Console.WriteLine("数据库连接信息配置错误");
                    return;
                }
                switch (dbtype)
                {
                    case "Oracle":
                        opt.UseOracle(dbstr);
                        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        break;
                    case "MySql":
                        opt.UseMySql(dbstr, MySqlServerVersion.LatestSupportedServerVersion);
                        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        break;
                    default:
                        Console.WriteLine($"暂时不支持{dbtype}数据库");
                        break;
                }
            });
            //仓储注册
            services.RepositoryRegister();
        }
    }
}
