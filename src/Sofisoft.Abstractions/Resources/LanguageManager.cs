using System.Collections.Concurrent;
using System.Globalization;

namespace Sofisoft.Abstractions.Resources
{
    public class LanguageManager : ILanguageManager
    {
        private readonly ConcurrentDictionary<string, string> _languages = new ConcurrentDictionary<string, string>();
        public bool Enabled { get; set; } = true;
        public CultureInfo? Culture { get; set; }
        

        private static string? GetTranslation(string culture, string key)
        {
			return culture switch {
				EnglishLanguage.Culture => EnglishLanguage.GetTranslation(key),
				SpanishLanguage.Culture => SpanishLanguage.GetTranslation(key),
				_ => null,
			};
		}

        public virtual string GetString(string key, CultureInfo? culture = null)
        {
			string value;

			if (Enabled)
            {
				culture = culture ?? Culture ?? CultureInfo.CurrentUICulture;

                var currentCulture = culture;
				string currentCultureKey = culture.Name + ":" + key;
				value = _languages.GetOrAdd(currentCultureKey, k => GetTranslation(culture.Name, key) ?? string.Empty);
				
				while (value == null && currentCulture.Parent != CultureInfo.InvariantCulture)
                {
					currentCulture = currentCulture.Parent;
					string parentCultureKey = currentCulture.Name + ":" + key;
					value = _languages.GetOrAdd(parentCultureKey, k => GetTranslation(currentCulture.Name, key) ?? string.Empty);
				}

				if (value == null
                    && culture.Name != EnglishLanguage.Culture
                    && !culture.IsNeutralCulture
                    && culture.Parent.Name != EnglishLanguage.Culture)
                {
					value = _languages.GetOrAdd(EnglishLanguage.Culture + ":" + key, k => EnglishLanguage.GetTranslation(key) ?? string.Empty);
				}
			}
			else
            {
				value = _languages.GetOrAdd(EnglishLanguage.Culture + ":" + key, k => EnglishLanguage.GetTranslation(key) ?? string.Empty);
			}

			return value ?? string.Empty;
		}
    }
}