using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace OriginFrameWork.UnitOfWorkModule;

public class OriginUnitOfWork<TDbContext> : IOriginUnitOfWork<TDbContext> where TDbContext : DbContext
{
    public TDbContext DbContext { get; set; }
    //事务
    private IDbContextTransaction _dbContextTransaction;
    public OriginUnitOfWork(TDbContext Context)
    {
        this.DbContext = Context;
    }
    /// <summary>
    /// 开启事务
    /// </summary>
    public void BeginTransaction()
    {
        _dbContextTransaction = DbContext.Database.BeginTransaction();
    }
    /// <summary>
    /// 事务的提交和回滚
    /// </summary>
    /// <returns></returns>
    public int Commit()
    {
        int result = 0;
        try
        {
            result = DbContext.SaveChanges();
            if (_dbContextTransaction != null)
            {
                _dbContextTransaction.Commit();
            }
        }
        catch (Exception ex)
        {
            result = -1;
            _dbContextTransaction.Rollback();
            throw new Exception($"Commit异常：{ex.InnerException}/r{ex.Message}");
        }
        return result;
    }
    /// <summary>
    /// 资源释放
    /// </summary> <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        _dbContextTransaction?.Dispose();
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }

}
