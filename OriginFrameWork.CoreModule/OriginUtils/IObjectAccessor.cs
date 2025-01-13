namespace OriginFrameWork.CoreModule.OriginUtils
{
    public interface IObjectAccessor<out T>
    {
        T? Value { get; }
    }
}
