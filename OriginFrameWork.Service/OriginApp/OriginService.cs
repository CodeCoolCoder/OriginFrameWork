namespace OriginFrameWork.Service.OriginApp;

public class OriginService : IOriginService
{


    public OriginService()
    {

    }
    public async Task<string> GetString(string get)
    {
        return get;
    }


}
