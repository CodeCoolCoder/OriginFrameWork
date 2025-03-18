using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace OriginFrameWork.EntityFrameWorkCoreModule.BaseProvider;


/// <summary>
/// 仓储层，与数据库交互的最底层
/// </summary>
public class BaseRepository<TModel, TDbContext> : IBaseRepository<TModel, TDbContext>
 where TModel : class where TDbContext : DbContext
{


    public BaseRepository(TDbContext tdbcontext)
    {
        Tdbcontext = tdbcontext;
    }
    public TDbContext OriginDbContext => Tdbcontext;
    public DbSet<TModel> OriginModel => OriginDbContext.Set<TModel>();

    public TDbContext Tdbcontext { get; }


    /// <summary>
    /// 通用接口，暴露出dbset，然后进行数据的筛选等操作    
    /// </summary>
    /// <returns></returns>
    public IQueryable<TModel> FindAsIQueryable()
    {
        return OriginModel;
    }
    /// <summary>
    /// 单条数据插入
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int OriginInsert(TModel t)
    {
        OriginModel.Add(t);
        return OriginDbContext.SaveChanges();
    }
    /// <summary>
    /// 批量数据插入
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int OriginInsertList(List<TModel> t)
    {
        OriginModel.AddRange(t);
        return OriginDbContext.SaveChanges();
    }
    /// <summary>
    /// 删除单条数据
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int OriginDelete(TModel t)
    {
        var res = OriginModel.Remove(t).Entity;
        var result = OriginDbContext.SaveChanges();
        return result;
    }
    /// <summary>
    /// 批量删除数据
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int OriginDeleteList(List<TModel> t)
    {
        OriginModel.RemoveRange(t);
        var result = OriginDbContext.SaveChanges();
        return result;
    }
    /// <summary>
    /// 单条数据更新
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int OriginUpdate(TModel t)
    {
        OriginModel.Update(t);
        return OriginDbContext.SaveChanges();
    }
    /// <summary>
    /// 批量数据更新
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int OriginUpdateList(List<TModel> t)
    {
        OriginModel.UpdateRange(t);
        return OriginDbContext.SaveChanges();
    }
    /// <summary>
    /// 查询单条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public TModel OriginGet(Expression<Func<TModel, bool>> expression)
    {
        return OriginModel.FirstOrDefault(expression);
    }
    /// <summary>
    /// 批量查询数据，不分页
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public IQueryable<TModel> OriginGetList(Expression<Func<TModel, bool>> expression)
    {
        return OriginModel.Where(expression);
    }
    /// <summary>
    /// 批量数据查询，分页
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="pageWithSortDto"></param>
    /// <returns></returns>
    public IQueryable<TModel> OriginGetList(Expression<Func<TModel, bool>> expression, PageWithSortDto pageWithSortDto)
    {
        int skip = (pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize;
        ///分页查询且排序
        if (pageWithSortDto.IsSort == true)
        {
            //升序排序
            if (pageWithSortDto.OrderType == OrderType.Asc)
            {
                return OriginModel.Where(expression).OrderBy(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize);
            }
            else
            {
                //降序排序
                return OriginModel.Where(expression).OrderByDescending(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize);
            }

        }
        else
        {
            /// <summary>
            /// 分页查询，不排序
            /// </summary>
            /// <returns></returns>
            return OriginModel.Where(expression).Skip(skip).Take(pageWithSortDto.PageSize);
        }
    }
    /// <summary>
    /// 使用原生sql查询数据
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public IQueryable<TModel> OriginGetForSQL(string sql, params object[] parameters)
    {
        return OriginModel.FromSqlRaw(sql, parameters);
    }
    /// <summary>
    /// 使用原生sql执行增删改查
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public int OriginExcuteSql(string sql, params object[] parameters)
    {
        return OriginDbContext.Database.ExecuteSqlRaw(sql, parameters);
    }
}
