using static Sofisoft.Abstractions.SofisoftConstants;

namespace Sofisoft.Abstractions.Resources
{
	internal static class SpanishLanguage
    {
		public const string Culture = "es";

		public static string? GetTranslation(string key) => key switch {
			HttpMethodError.Default => "Se produjo un error al consumir el servicio.",
			HttpMethodError.Delete => "Se produjo un error al eliminar el recurso",
			HttpMethodError.Get => "Se produjo un error obteniendo el recurso.",
			HttpMethodError.Patch => "Se produjo un error al intentar actualizar el recurso.",
			HttpMethodError.Post => "Se produjo un error al intentar crear el recurso.",
			HttpMethodError.Put => "Se produjo un error al intentar actualizar el recurso.",
			_ => null,
		};
	}
}