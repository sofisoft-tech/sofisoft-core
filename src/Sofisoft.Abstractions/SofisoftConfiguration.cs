using Sofisoft.Abstractions.Resources;

namespace Sofisoft.Abstractions
{
    public class SofisoftConfiguration
    {
        private ILanguageManager _languageManager = new LanguageManager();
    
        public ILanguageManager LanguageManager
        {
			get => _languageManager;
			set => _languageManager = value ?? throw new ArgumentNullException(nameof(value));
		}
    }
}