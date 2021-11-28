using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Sofisoft.Logging.Tests
{
    public class SofisoftLoggingBuilderTest
    {
        [Fact]
        public void Constructor_ThrowsAnExceptionForNullServices()
        {
            // Arrange
            var services = (IServiceCollection) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new SofisoftLoggingBuilder(services));

            // Assert
            Assert.Equal("services", exception.ParamName);
        }
        
        [Fact]
        public void Configure_ThrowsAnExceptionForNullConfiguration()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.Configure(null!));

            // Assert
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void SetBaseAddress_throws_an_exception_for_null_parameter_with_uri()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            var uri = (Uri) null!;
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetBaseAddress(uri));

            // Assert
            Assert.Equal("baseAddress", exception.ParamName);
        }

        [Fact]
        public void SetBaseAddress_is_set_with_uri()
        {
            // Arrange
            var expectedUri = new Uri("http://localhost");
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetBaseAddress(expectedUri);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedUri, options.BaseAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetBaseAddress_throws_an_exception_for_null_parameter(string baseAddress)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetBaseAddress(baseAddress));

            // Assert
            Assert.Equal("baseAddress", exception.ParamName);
        }

        [Fact]
        public void SetBaseAddress_throws_an_exception_for_not_uri()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentException>(() => builder.SetBaseAddress("baduri"));

            // Assert
            Assert.Equal("baseAddress", exception.ParamName);
        }

        [Fact]
        public void SetBaseAddress_is_set_with_string_uri()
        {
            // Arrange
            var expectedUri = new Uri("http://localhost");
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetBaseAddress("http://localhost");

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedUri, options.BaseAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetErrorEndPointUri_throws_an_exception_for_null_parameter(string endpoint)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetErrorEndPointUri(endpoint));

            // Assert
            Assert.Equal("endpoint", exception.ParamName);
        }

        [Fact]
        public void SetErrorEndPointUri_is_set()
        {
            // Arrange
            var expectedEndPoint = "api/error";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetErrorEndPointUri(expectedEndPoint);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedEndPoint, options.ErrorEndPointUri);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetInformationEndPointUri_throws_an_exception_for_null_parameter(string endpoint)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetInformationEndPointUri(endpoint));

            // Assert
            Assert.Equal("endpoint", exception.ParamName);
        }

        [Fact]
        public void SetInformationEndPointUri_is_set()
        {
            // Arrange
            var expectedEndPoint = "api/information";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetInformationEndPointUri(expectedEndPoint);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedEndPoint, options.InformationEndPointUri);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetSource_throws_an_exception_for_null_parameter(string source)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetSource(source));

            // Assert
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void SetSource_is_set()
        {
            // Arrange
            var expectedSource = "api";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetSource(expectedSource);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedSource, options.Source);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetTokenType_throws_an_exception_for_null_parameter(string type)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetTokenType(type));

            // Assert
            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public void SetTokenType_is_set()
        {
            // Arrange
            var expectedTokenType = "test";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetTokenType(expectedTokenType);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedTokenType, options.TokenType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetTokenValue_throws_an_exception_for_null_parameter(string value)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetTokenValue(value));

            // Assert
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void SetTokenValue_is_set()
        {
            // Arrange
            var expectedTokenValue = "token";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetTokenValue(expectedTokenValue);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedTokenValue, options.TokenValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetWarningEndPointUri_throws_an_exception_for_null_parameter(string endpoint)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetWarningEndPointUri(endpoint));

            // Assert
            Assert.Equal("endpoint", exception.ParamName);
        }

        [Fact]
        public void SetWarningEndPointUri_is_set()
        {
            // Arrange
            var expectedEndPoint = "api/warning";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetWarningEndPointUri(expectedEndPoint);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftLoggingOptions>>().CurrentValue;

            Assert.Equal(expectedEndPoint, options.WarningEndPointUri);
        }

        private static SofisoftLoggingBuilder CreateBuilder(IServiceCollection services)
            => services.AddSofisoft().AddLogging();

        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();

            return services;
        }

    }
}