using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Sofisoft.Identity.Tests
{
    public class SofisoftIdentityBuilderTest
    {
        [Fact]
        public void Constructor_ThrowsAnExceptionForNullServices()
        {
            // Arrange
            var services = (IServiceCollection) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new SofisoftIdentityBuilder(services));

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetClientIdKey_throws_an_exception_for_null_parameter(string key)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetClientIdKey(key));

            // Assert
            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void SetClientIdKey_is_set()
        {
            // Arrange
            var expectedSource = "client_id";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetClientIdKey(expectedSource);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftIdentityOptions>>().CurrentValue;

            Assert.Equal(expectedSource, options.ClientIdKey);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetCompanyIdKey_throws_an_exception_for_null_parameter(string key)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetCompanyIdKey(key));

            // Assert
            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void SetCompanyIdKey_is_set()
        {
            // Arrange
            var expectedSource = "company_id";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetCompanyIdKey(expectedSource);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftIdentityOptions>>().CurrentValue;

            Assert.Equal(expectedSource, options.CompanyIdKey);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetUserIdKey_throws_an_exception_for_null_parameter(string key)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetUserIdKey(key));

            // Assert
            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void SetUserIdKey_is_set()
        {
            // Arrange
            var expectedSource = "user_id";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetUserIdKey(expectedSource);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftIdentityOptions>>().CurrentValue;

            Assert.Equal(expectedSource, options.UserIdKey);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetUsernameKey_throws_an_exception_for_null_parameter(string key)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetUsernameKey(key));

            // Assert
            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void SetUsernameKey_is_set()
        {
            // Arrange
            var expectedSource = "username";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetUsernameKey(expectedSource);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftIdentityOptions>>().CurrentValue;

            Assert.Equal(expectedSource, options.UsernameKey);
        }

        private static SofisoftIdentityBuilder CreateBuilder(IServiceCollection services)
            => services.AddSofisoft().AddIdentity();

        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();

            return services;
        }
    }
}