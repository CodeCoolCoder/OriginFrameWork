using OriginFrameWork.RemoteInvokeModule;
using OriginFrameWork.RemoteInvokeModule.RemoteAttributes;


namespace OriginFrameWork.Service.OriginApp;
[RemoteServiceAttribute("OriginService")]
public interface IOriginService : IRemoteServiceTag
{
    [RemoteServiceIndividualAttribute("GET")]
    Task<string> GetString(string get);
}
