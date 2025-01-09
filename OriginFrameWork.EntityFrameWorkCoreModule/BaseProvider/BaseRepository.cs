using Microsoft.EntityFrameworkCore;

using OriginFrameWork.CoreModule.OriginInterface;
using System.Linq.Expressions;


namespace OriginFrameWork.EntityFrameWorkCoreModule.BaseProvider;


/// <summary>
/// 仓储层，与数据库交互的最底层
/// </summary>
public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : BaseModel
{
    // public OriginFrameWorkDbContext OriginDbContext;
    // private OriginFrameWorkDbContext _originFrameWorkDbContext;
    //  public OriginFrameWorkDbContext OriginDbContext => _originFrameWorkDbContext;
    //=> OriginDbContext.Set<TModel>()
    //public OriginFrameWorkDbContext OriginDbContext { get; set; }
    //public IOriginUnitOfWork<OriginFrameWorkDbContext> OriginUnitOfWork { get; }
    //public DbSet<TModel> OriginModel ;
    //public BaseRepository(OriginFrameWorkDbContext originFrameWorkDbContext, 
    //    IOriginUnitOfWork<OriginFrameWorkDbContext> originUnitOfWork)
    //{
    //    OriginDbContext = originFrameWorkDbContext;
    //    OriginUnitOfWork = originUnitOfWork;
    //    OriginModel = OriginDbContext.Set<TModel>();
    //    //this._originFrameWorkDbContext = originFrameWorkDbContext;
    //    var workunitTypeList = OriginModel.EntityType.ClrType.GetCustomAttributes(typeof(OriginUnitOfWorkAttribute), false).ToList();
    //    workunitTypeList.ForEach(x =>
    //    {
    //        if (x.GetType() == typeof(OriginUnitOfWorkAttribute))
    //        {
    //            var unitEntity = x as OriginUnitOfWorkAttribute;
    //            if (unitEntity.IsTurnOn)
    //            {
    //                // this._originFrameWorkDbContext = OriginUnitOfWork.DbContext;
    //                OriginDbContext = OriginUnitOfWork.DbContext;
    //                OriginModel = OriginDbContext.Set<TModel>();
    //            }
    //            else
    //            {
    //                // this._originFrameWorkDbContext = originFrameWorkDbContext;
    //                OriginDbContext = originFrameWorkDbContext;
    //                OriginModel = OriginDbContext.Set<TModel>();
    //            }
    //        }
    //    });

    //}

    private readonly OriginDbContext _originFrameWorkDbContext;

    public BaseRepository(OriginDbContext originFrameWorkDbContext)
    {
        _originFrameWorkDbContext = originFrameWorkDbContext;
    }
    public OriginDbContext OriginDbContext => _originFrameWorkDbContext;
    public DbSet<TModel> OriginModel => OriginDbContext.Set<TModel>();


    /// <summary>
    /// 通用接口，暴露出dbset，然后进行数据的筛选等操作    
    /// </summary>
    /// <returns></returns>
    public IQueryable<TModel> FindAsIQueryable()
    {
        return OriginModel;
    }


    // public void OriginTransaction(Action action)
    // {

    //     var res = OriginDbContext.Database.BeginTransaction();

    // }


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
    public async Task<int> OriginInsertAsync(TModel t)
    {
        await OriginModel.AddAsync(t);
        return await OriginDbContext.SaveChangesAsync();
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
    public async Task<int> OriginInsertListAsync(List<TModel> t)
    {
        await OriginModel.AddRangeAsync(t);
        return await OriginDbContext.SaveChangesAsync();
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

    public async Task<int> OriginDeleteAsync(TModel t)
    {
        var res = OriginModel.Remove(t).Entity;
        var result = await OriginDbContext.SaveChangesAsync();
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
    public async Task<int> OriginDeleteListAsync(List<TModel> t)
    {
        OriginModel.RemoveRange(t);
        var result = await OriginDbContext.SaveChangesAsync();
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
    public async Task<int> OriginUpdateAsync(TModel t)
    {
        OriginModel.Update(t);
        return await OriginDbContext.SaveChangesAsync();
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
    public async Task<int> OriginUpdateListAsync(List<TModel> t)
    {
        OriginModel.UpdateRange(t);
        return await OriginDbContext.SaveChangesAsync();
    }
    /// <summary>
    /// 查询单条数据
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public TModel OriginGet(Expression<Func<TModel, bool>> expression)
    {
        return OriginModel.Where(expression).FirstOrDefault();
    }
    public async Task<TModel> OriginGetAsync(Expression<Func<TModel, bool>> expression)
    {
        return await OriginModel.Where(expression).FirstOrDefaultAsync();
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
    public async Task<List<TModel>> OriginGetListAsync(Expression<Func<TModel, bool>> expression)
    {
        return await OriginModel.Where(expression).ToListAsync();
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
    public async Task<List<TModel>> OriginGetListAsync(Expression<Func<TModel, bool>> expression, PageWithSortDto pageWithSortDto)
    {
        int skip = (pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize;
        ///分页查询且排序
        if (pageWithSortDto.IsSort == true)
        {
            //升序排序
            if (pageWithSortDto.OrderType == OrderType.Asc)
            {
                return await OriginModel.Where(expression).OrderBy(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
            }
            else
            {
                //降序排序
                return await OriginModel.Where(expression).OrderByDescending(m => pageWithSortDto.Sort).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
            }

        }
        else
        {
            /// <summary>
            /// 分页查询，不排序
            /// </summary>
            /// <returns></returns>
            return await OriginModel.Where(expression).Skip(skip).Take(pageWithSortDto.PageSize).ToListAsync();
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
    public async Task<List<TModel>> OriginGetForSQLAsync(string sql, params object[] parameters)
    {
        return await OriginModel.FromSqlRaw(sql, parameters).ToListAsync();
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
    public async Task<int> OriginExcuteSqlAsync(string sql, params object[] parameters)
    {

        return await OriginDbContext.Database.ExecuteSqlRawAsync(sql, parameters);
    }


}
