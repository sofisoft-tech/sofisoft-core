using System.Globalization;
using Sofisoft.Abstractions.Resources;
using Xunit;

namespace Sofisoft.Abstractions.Tests
{
    public class LanguageManagerTest
    {
        [Theory]
        [InlineData("Default", "en")]
        [InlineData("Delete", "en")]
        [InlineData("Get", "en")]
        [InlineData("Patch", "en")]
        [InlineData("Post", "en")]
        [InlineData("Put", "en")]
        [InlineData("Default", "es")]
        [InlineData("Delete", "es")]
        [InlineData("Get", "es")]
        [InlineData("Patch", "es")]
        [InlineData("Post", "es")]
        [InlineData("Put", "es")]
        public void GetString_when_enabled_return_not_empty(string key, string name)
        {
            // Arrange
            CultureInfo culture = new CultureInfo(name);
            var languageManager = new LanguageManager();
            languageManager.Enabled = true;

            // Act
            var result = languageManager.GetString(key, culture);

            // Assert
            Assert.NotEqual(string.Empty, result);
        }

        [Theory]
        [InlineData("Default", "es")]
        public void GetString_when_not_enable_return_english(string key, string name)
        {
            // Arrange
            CultureInfo culture = new CultureInfo(name);
            var languageManager = new LanguageManager();
            languageManager.Enabled = false;

            // Act
            var result = languageManager.GetString(key, culture);

            // Assert
            Assert.NotEqual(string.Empty, result);
        }

        [Theory]
        [InlineData("Anywhere", "en")]
        [InlineData("Anywhere", "es")]
        public void GetString_when_not_key_found_return_empty(string key, string name)
        {
            // Arrange
            CultureInfo culture = new CultureInfo(name);
            var languageManager = new LanguageManager();

            // Act
            var result = languageManager.GetString(key, culture);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }

}