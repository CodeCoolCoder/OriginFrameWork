using Microsoft.EntityFrameworkCore;
using OriginFrameWork.Entity;

namespace OriginFrameWork.Core.UnitOfWork;

public interface IOriginUnitOfWork<TDbContext> :IocTagForGenerics where TDbContext : DbContext
{
    public TDbContext DbContext { get; set; }
    void BeginTransaction();
    int Commit();
    public void Dispose();
}
