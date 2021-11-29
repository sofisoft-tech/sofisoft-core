using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Identity.Managers;
using Xunit;

namespace Sofisoft.Identity.Tests
{
    public class SofisoftIdentityExtensionsTest
    {
        [Fact]
        public void AddIdentity_throws_an_exception_for_null_builder()
        {
            // Arrange
            var builder = (SofisoftBuilder) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddIdentity());

            // Assert
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddIdentity_throws_an_exception_for_null_builder_in_configuration()
        {
            // Arrange
            var builder = (SofisoftBuilder) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddIdentity(configuration: null!));

            // Assert
            Assert.Equal("builder", exception.ParamName);

        }

        [Fact]
        public void AddIdentity_throws_an_exception_for_null_configuration()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddIdentity(configuration: null!));

            // Assert
            Assert.Equal("configuration", exception.ParamName);
        }

        [Theory]
        [InlineData(typeof(IHttpContextAccessor), typeof(HttpContextAccessor))]
        [InlineData(typeof(IIdentityManager), typeof(IdentityManager))]
        public void AddIdentity_register_types(Type serviceType, Type implementationType)
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = new SofisoftBuilder(services);

            // Act
            builder.AddIdentity();

            // Assert
            Assert.Contains(services, service => service.ServiceType == serviceType &&
                                                service.ImplementationType == implementationType);
        }

        [Fact]
        public void AddIdentity_set_options()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.AddIdentity(options =>
            {
                options.SetClientIdKey("client");
                options.SetCompanyIdKey("company");
                options.SetUserIdKey("id");
                options.SetUsernameKey("name");
            });

            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftIdentityOptions>>();

            // Assert
            Assert.Equal("client", options.CurrentValue.ClientIdKey);
            Assert.Equal("company", options.CurrentValue.CompanyIdKey);
            Assert.Equal("id", options.CurrentValue.UserIdKey);
            Assert.Equal("name", options.CurrentValue.UsernameKey);
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