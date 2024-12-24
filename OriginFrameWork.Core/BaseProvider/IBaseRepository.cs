using System.Linq.Expressions;
using OriginFrameWork.Entity;

namespace OriginFrameWork.Core.BaseProvider;

public interface IBaseRepository<TModel> where TModel : BaseModel
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
}
