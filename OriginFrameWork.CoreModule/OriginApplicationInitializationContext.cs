using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace OriginFrameWork.CoreModule
{
    public class OriginApplicationInitializationContext
    {
        public WebApplication App { get; set; }

        public OriginApplicationInitializationContext([NotNull] WebApplication app)
        {
            App = app;
        }
    }
}
