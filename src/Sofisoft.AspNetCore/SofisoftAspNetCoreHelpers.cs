using Microsoft.Extensions.DependencyInjection;
using Sofisoft.Abstractions;

namespace Sofisoft.AspNetCore
{
    public static class SofisoftAspNetCoreHelpers
    {
        public static SofisoftConfiguration GetSofisoftConfiguration(this IServiceProvider serviceProvider)
        {
			return serviceProvider.GetRequiredService<SofisoftConfiguration>();
		}
    }
}