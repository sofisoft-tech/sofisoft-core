using Microsoft.Extensions.DependencyInjection;
using Sofisoft.Abstractions;

namespace Sofisoft.Logging
{
    public static class SofisoftLoggingHelpers
    {
        public static SofisoftConfiguration GetSofisoftConfiguration(this IServiceProvider serviceProvider)
        {
			return serviceProvider.GetRequiredService<SofisoftConfiguration>();
		}
    }
}