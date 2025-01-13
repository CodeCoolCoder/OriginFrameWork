using OriginFrameWork.CoreModule.OriginInterface;

namespace OriginFrameWork.Service.OriginApp
{
    public interface IGetDbTest : IocTag
    {
        List<string> GetMainDevices();
    }
}
