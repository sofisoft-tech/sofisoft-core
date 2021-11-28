using System.Globalization;

namespace Sofisoft.Abstractions.Resources
{
    /// <summary>
	/// Allows the default error message translations to be managed.
	/// </summary>
    public interface ILanguageManager
    {
        /// <summary>
		/// Whether localization is enabled.
		/// </summary>
		bool Enabled { get; set; }

        /// <summary>
		/// Default culture to use for all requests to the LanguageManager. If not specified, uses the current UI culture.
		/// </summary>
		CultureInfo? Culture { get; set; }

        /// <summary>
		/// Gets a translated string based on its key. If the culture is specific and it isn't registered, we try the neutral culture instead.
		/// If no matching culture is found  to be registered we use English.
		/// </summary>
		/// <param name="key">The key</param>
		/// <param name="culture">The culture to translate into</param>
		/// <returns></returns>
		string GetString(string key, CultureInfo? culture = null);
    }
}