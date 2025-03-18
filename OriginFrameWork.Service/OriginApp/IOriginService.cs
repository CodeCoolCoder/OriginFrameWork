
using OriginFrameWork.CoreModule.OriginInterface;
using OriginFrameWork.RemoteInvokeModule.RemoteAttributes;

namespace OriginFrameWork.Service.OriginApp;
[RemoteServiceAttribute("OriginService")]
public interface IOriginService : IRemoteServiceTag
{
    [RemoteServiceIndividualAttribute("POST")]
    Task<string> GetString(string get);

}
