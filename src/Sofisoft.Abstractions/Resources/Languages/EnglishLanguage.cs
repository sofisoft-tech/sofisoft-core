using static Sofisoft.Abstractions.SofisoftConstants;

namespace Sofisoft.Abstractions.Resources
{
	internal static class EnglishLanguage
    {
		public const string Culture = "en";

		public static string? GetTranslation(string key) => key switch {
			HttpMethodError.Default => "An error occurred while consuming the service.",
			HttpMethodError.Delete => "An error occurred while deleting the resource.",
			HttpMethodError.Get => "An error has occurred obtaining the resource.",
			HttpMethodError.Patch => "An error occurred while trying to update the resource.",
			HttpMethodError.Post => "An error occurred while trying to create the resource.",
			HttpMethodError.Put => "An error occurred while trying to update the resource.",
			_ => null,
		};
	}
}
