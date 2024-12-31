namespace OriginFrameWork.Service.OriginApp
{
    public class OriginService : IOriginService
    {
        public async Task<string> GetString(string get)
        {
            return await Task.FromResult(get);
        }
    }
}
