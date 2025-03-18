using Microsoft.EntityFrameworkCore;

namespace OriginFrameWork.EntityFrameWorkCoreModule
{
    /// <summary>
    /// dbcontxt工厂
    /// </summary>
    public static class OriginDbContextFactory
    {
        public static DbContextOptionsBuilder ConfigureDbContext(
            DbContextOptionsBuilder optionsBuilder,
            string dbType,
            string connectionString)
        {
            switch (dbType?.ToLower())
            {
                case "oracle":
                    optionsBuilder.UseOracle(connectionString);
                    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
                case "mysql":
                    optionsBuilder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion);
                    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
                case "sqlserver":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                default:
                    Console.WriteLine($"暂时不支持{dbType}数据库");
                    break;
            }

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            return optionsBuilder;
        }
    }


}
