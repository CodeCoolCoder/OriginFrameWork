using System.Reflection;
using System.Runtime.Loader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using OriginFrameWork.Entity;

namespace OriginFrameWork.Core.EFDbContext;

/// <summary>
/// 数据库上下文
/// </summary>
public class OriginFrameWorkDbContext : DbContext
{
    public OriginFrameWorkDbContext()
    {

    }
    public OriginFrameWorkDbContext(DbContextOptions options) : base(options)
    {

    }
    public static readonly ILoggerFactory loggerFactory
     = LoggerFactory.Create(builder => { builder.AddConsole(); });

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(loggerFactory);
        // if (!optionsBuilder.IsConfigured)
        // {
        //     optionsBuilder.UseOracle("数据库连接字符串");
        //     optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        // }
        base.OnConfiguring(optionsBuilder);
    }
    /// <summary>
    /// 通过反射自动创建dbset
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //var compilationlibrary
        try
        {
            //获取所有的类库
            //依赖Microsoft.Extensions.DependencyModel 
            var compilationlibrary = DependencyContext.Default.CompileLibraries.
            Where(x => !x.Serviceable && x.Type != "package" && x.Type == "project");
            //获取所有的数据库模型
            //首先循环程序集，然后再对继承basemodel的所有类型进 行循环 
            foreach (var _compilationlibrary in compilationlibrary)
            {
                AssemblyLoadContext.Default.LoadFromAssemblyName(new System.Reflection.
                AssemblyName(_compilationlibrary.Name))
                .GetTypes().Where(x => x.GetTypeInfo().BaseType != null && !x.IsAbstract
                && x.BaseType == typeof(BaseModel))
                .ToList().ForEach(t => modelBuilder.Entity(t));
            }
        }
        catch (System.Exception)
        {
            throw;
        }
        base.OnModelCreating(modelBuilder);
    }
}
