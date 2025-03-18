using OriginFrameWork.EntityFrameWork;
using OriginFrameWork.EntityFrameWorkCoreModule.BaseProvider;

namespace OriginFrameWork.Service.OriginApp
{
    public class GetDbTest : IGetDbTest
    {
        public GetDbTest(IBaseRepository<MainDevice, OriginDbContext> baseRepository)
        {
            BaseRepository = baseRepository;
        }

        public IBaseRepository<MainDevice, OriginDbContext> BaseRepository { get; }

        public List<string> GetMainDevices()
        {
            return BaseRepository.FindAsIQueryable().Select(m => m.MaterialName).ToList();
        }
    }
}
