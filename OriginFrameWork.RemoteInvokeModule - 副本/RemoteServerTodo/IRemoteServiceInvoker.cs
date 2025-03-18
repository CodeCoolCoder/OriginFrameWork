namespace OriginFrameWork.RemoteInvokeModule.RemoteServerTodo
{
    public interface IRemoteServiceInvoker
    {
        Task<TResult> InvokeAsync<TResult>(RemoteServiceInvocationContext context);
    }
}