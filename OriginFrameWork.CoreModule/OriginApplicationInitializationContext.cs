using System.Diagnostics.CodeAnalysis;

namespace OriginFrameWork.CoreModule
{
    public class OriginApplicationInitializationContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public OriginApplicationInitializationContext([NotNull] IServiceProvider serviceProvider)
        {

            ServiceProvider = serviceProvider;
        }
    }
}
