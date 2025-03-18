using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OriginFrameWork.EntityFrameWorkCoreModule.BaseProvider;

public interface IBaseRepository<TModel, TDbContext> where TDbContext : DbContext
{
    IQueryable<TModel> FindAsIQueryable();
    int OriginDelete(TModel t);
    int OriginDeleteList(List<TModel> t);
    TModel OriginGet(Expression<Func<TModel, bool>> expression);
    IQueryable<TModel> OriginGetList(Expression<Func<TModel, bool>> expression);
    IQueryable<TModel> OriginGetList(Expression<Func<TModel, bool>> expression, PageWithSortDto pageWithSortDto);
    int OriginInsert(TModel t);
    int OriginInsertList(List<TModel> t);
    int OriginUpdate(TModel t);
    int OriginUpdateList(List<TModel> t);
    IQueryable<TModel> OriginGetForSQL(string sql, params object[] parameters);
    int OriginExcuteSql(string sql, params object[] parameters);
}
