namespace OriginFrameWork.UnitOfWorkModule;

public interface IOriginUnitOfWork<TDbContext>
{
    void BeginTransaction();
    int Commit();
    public void Dispose();
}
