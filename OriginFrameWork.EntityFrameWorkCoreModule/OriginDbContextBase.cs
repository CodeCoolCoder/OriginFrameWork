using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using OriginFrameWork.CoreModule.OriginConfigCore;
using OriginFrameWork.CoreModule.OriginInterface;
using System.Reflection;
using System.Runtime.Loader;

namespace OriginFrameWork.EntityFrameWorkCoreModule
{
    public abstract class OriginDbContextBase : DbContext
    {
        protected readonly ILoggerFactory _loggerFactory;
        protected string _connectionString;
        protected string _dbType;

        // 添加静态属性来存储配置文件路径
        private static string _configPath;

        /// <summary>
        /// 设置配置文件路径
        /// </summary>
        /// <param name="configPath">配置文件的完整路径</param>
        public static void SetConfigurationPath(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
                throw new ArgumentNullException(nameof(configPath));

            if (!File.Exists(configPath))
                throw new FileNotFoundException("配置文件不存在", configPath);

            _configPath = configPath;
        }
        public OriginDbContextBase()
        {

        }

        public OriginDbContextBase(DbContextOptions options) : base(options)
        {
            _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 获取配置
                var configuration = GetConfiguration();
                var connection = configuration.GetSection("Connection").Get<Connection>();

                if (connection == null)
                    throw new InvalidOperationException("未能找到数据库连接配置");

                _connectionString = connection.DbConnectionString;
                _dbType = connection.DbType;
                if (string.IsNullOrEmpty(_dbType) || string.IsNullOrEmpty(_connectionString))
                {
                    throw new InvalidOperationException("数据库连接信息配置错误");
                }
                ConfigureDatabase(optionsBuilder, _dbType, _connectionString);
            }
            // 添加日志工厂
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }
        protected virtual IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder();

            // 如果设置了自定义配置文件路径，则使用它
            if (!string.IsNullOrEmpty(_configPath))
            {
                builder.SetBasePath(Path.GetDirectoryName(_configPath))
                      .AddJsonFile(Path.GetFileName(_configPath), optional: false, reloadOnChange: true);
            }
            else
            {
                // 默认查找路径顺序：
                // 1. 当前目录
                // 2. 应用程序根目录
                // 3. 自定义环境变量指定的目录
                var paths = new[]
                {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory,
                Environment.GetEnvironmentVariable("ORIGIN_CONFIG_PATH")
            }.Where(p => !string.IsNullOrEmpty(p)).Distinct();

                foreach (var path in paths)
                {
                    var configFile = Path.Combine(path, "appsettings.json");
                    if (File.Exists(configFile))
                    {
                        builder.SetBasePath(path)
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        break;
                    }
                }
            }

            // 添加环境特定的配置文件
            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            //var envConfigFile = $"appsettings.{environment}.json";

            //if (File.Exists(Path.Combine(builder.GetFileProvider().GetFileInfo("").PhysicalPath, envConfigFile)))
            //{
            //    builder.AddJsonFile(envConfigFile, optional: true, reloadOnChange: true);
            //}

            //// 添加环境变量
            //builder.AddEnvironmentVariables();

            return builder.Build();
        }
        /// <summary>
        /// 通过反射自动注册实体
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            RegisterEntities(modelBuilder);
        }
        /// <summary>
        /// 数据库类型相关信息配置
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        protected virtual void ConfigureDatabase(DbContextOptionsBuilder optionsBuilder, string dbType, string connectionString)
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
        }
        /// <summary>
        /// 注册实体的虚方法，允许子类重写以自定义实体注册逻辑
        /// </summary>
        protected virtual void RegisterEntities(ModelBuilder modelBuilder)
        {
            // 获取所有继承自BaseModel的实体类
            var entityTypes = GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                if (entityType.GetTypeInfo().BaseType == typeof(BaseModel))
                {
                    modelBuilder.Entity(entityType);
                }
            }
        }

        /// <summary>
        /// 获取实体类型的虚方法，允许子类重写以自定义实体类型的获取逻辑
        /// </summary>
        protected virtual IEnumerable<Type> GetEntityTypes()
        {
            var assemblies = GetAssemblies();
            return assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetTypeInfo().BaseType != null
                    && !type.IsAbstract
                    && type.BaseType == typeof(BaseModel));
        }

        /// <summary>
        /// 获取程序集的虚方法，允许子类重写以自定义程序集的获取逻辑
        /// </summary>
        protected virtual IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();

            var dependencies = DependencyContext.Default?.CompileLibraries
                .Where(x => !x.Serviceable && x.Type != "package" && x.Type == "project");

            if (dependencies != null)
            {
                foreach (var library in dependencies)
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default
                            .LoadFromAssemblyName(new AssemblyName(library.Name));
                        assemblies.Add(assembly);
                    }
                    catch (Exception)
                    {
                        // 记录日志但继续执行
                        continue;
                    }
                }
            }

            return assemblies;
        }
    }

}
