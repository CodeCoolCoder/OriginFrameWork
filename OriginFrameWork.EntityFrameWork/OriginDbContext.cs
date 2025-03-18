using Microsoft.EntityFrameworkCore;
using OriginFrameWork.EntityFrameWorkCoreModule;

namespace OriginFrameWork.EntityFrameWork
{
    public class OriginDbContext : OriginDbContextBase
    {
        public OriginDbContext() : base()
        {

        }
        public OriginDbContext(DbContextOptions<OriginDbContext> options)
      : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
        //// 可以选择性地重写以下方法来自定义行为
        //protected override void RegisterEntities(ModelBuilder modelBuilder)
        //{
        //    base.RegisterEntities(modelBuilder);
        //    // 添加自定义实体映射
        //}

        //protected override IEnumerable<Type> GetEntityTypes()
        //{
        //    // 自定义实体类型获取逻辑
        //    return base.GetEntityTypes();
        //}
    }
}
