using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.EntityFrameWorkCoreModule.BaseProvider;

namespace OriginFrameWork.EntityFrameWorkCoreModule.Microsoft.Extensions.DependencyInjection
{
    public static class AddOriginDbcontextExtension
    {
        public static IServiceCollection AddOriginDbContext<TDbContext>(
     this IServiceCollection services,
     Action<OriginDbContextOptions> setupAction = null)
     where TDbContext : OriginDbContextBase
        {
            var options = new OriginDbContextOptions();
            setupAction?.Invoke(options);

            // 如果提供了配置文件路径，设置它
            if (!string.IsNullOrEmpty(options.ConfigurationPath))
            {
                OriginDbContextBase.SetConfigurationPath(options.ConfigurationPath);
            }

            services.AddDbContext<TDbContext>(ServiceLifetime.Scoped);

            return services;
        }
        public static void AddBaseRepository(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        }
    }
}
