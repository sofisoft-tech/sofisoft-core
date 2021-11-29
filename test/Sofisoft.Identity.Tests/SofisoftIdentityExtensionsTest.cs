using System;
using Microsoft.Extensions.DependencyInjection;
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