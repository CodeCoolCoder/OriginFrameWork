using Microsoft.EntityFrameworkCore.Design;

namespace OriginFrameWork.EntityFrameWork
{
    public class OriginDbContextFactory : IDesignTimeDbContextFactory<OriginDbContext>
    {
        public OriginDbContext CreateDbContext(string[] args)
        {
            return new OriginDbContext();
        }
    }
}
