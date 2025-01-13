using OriginFrameWork.EntityFrameWork;
using OriginFrameWork.EntityFrameWorkCoreModule.BaseProvider;

namespace OriginFrameWork.Service.OriginApp
{
    public class GetDbTest : IGetDbTest
    {
        public GetDbTest(IBaseRepository<MainDevice> baseRepository)
        {
            BaseRepository = baseRepository;
        }

        public IBaseRepository<MainDevice> BaseRepository { get; }

        public List<string> GetMainDevices()
        {
            return BaseRepository.FindAsIQueryable().Select(m => m.MaterialName).ToList();
        }
    }
}
