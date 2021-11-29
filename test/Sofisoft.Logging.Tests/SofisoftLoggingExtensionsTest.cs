using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sofisoft.Logging;
using Xunit;

namespace Sofisoft.Abstractions.Tests
{
    public class SofisoftLoggingExtensionsTest
    {
        [Fact]
        public void AddLogging_throws_an_exception_for_null_builder()
        {
            // Arrange
            var builder = (SofisoftBuilder) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddLogging());

            // Assert
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddLogging_throws_an_exception_for_null_builder_in_configuration()
        {
            // Arrange
            var builder = (SofisoftBuilder) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddLogging(configuration: null!));

            // Assert
            Assert.Equal("builder", exception.ParamName);

        }

        [Fact]
        public void AddLogging_throws_an_exception_for_null_configuration()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddLogging(configuration: null!));

            // Assert
            Assert.Equal("configuration", exception.ParamName);

        }

        [Fact]
        public void AddLogging_set_options()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.AddLogging(options =>
            {
                options.SetBaseAddress("http://local");
                options.SetErrorEndPointUri("api/error");
                options.SetInformationEndPointUri("api/info");
                options.SetSource("this");
                options.SetTokenType("Basic");
                options.SetTokenValue("ABC=");
                options.SetWarningEndPointUri("api/warning");
            });

            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>();

            // Assert
            Assert.Equal(new Uri("http://local"), options.CurrentValue.BaseAddress);
            Assert.Equal("api/error", options.CurrentValue.ErrorEndPointUri);
            Assert.Equal("api/info", options.CurrentValue.InformationEndPointUri);
            Assert.Equal("this", options.CurrentValue.Source);
            Assert.Equal("Basic", options.CurrentValue.TokenType);
            Assert.Equal("ABC=", options.CurrentValue.TokenValue);
            Assert.Equal("api/warning", options.CurrentValue.WarningEndPointUri);
        }

        private static SofisoftBuilder CreateBuilder(IServiceCollection services)
            => services.AddSofisoft();
        
        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();

            return services;
        }
    }
}