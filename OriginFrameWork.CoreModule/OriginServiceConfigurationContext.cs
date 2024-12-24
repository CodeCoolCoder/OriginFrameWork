using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginFrameWork.CoreModule
{
    public class OriginServiceConfigurationContext
    {
        public IServiceCollection Services { get; }

        public IDictionary<string, object?> Items { get; }
        //public object? this[string key]
        //{
        //    get => Items.GetOrDefault(key);
        //    set => Items[key] = value;
        //}

        public OriginServiceConfigurationContext([NotNull] IServiceCollection services)
        {
            Services=services;
            // Services = Check.NotNull(services, nameof(services));
            Items = new Dictionary<string, object?>();
        }
    }
}
