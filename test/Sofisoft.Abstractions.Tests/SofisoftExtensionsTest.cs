using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Sofisoft.Abstractions.Tests
{
    public class SofisoftExtensionsTest
    {
        [Fact]
        public void AddSofisoft_throws_an_exception_for_null_services()
        {
            // Arrange
            var services = (IServiceCollection) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => services.AddSofisoft());

            // Assert
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddSofisoft_throws_an_exception_for_null_configuration()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => services.AddSofisoft(configuration: null!));

            // Assert
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void AddSofisoft_register_sofisoftconfiguration()
        {
            // Arrange
            var services = new ServiceCollection();
            // var builder = new SofisoftBuilder(services);

            // Act
            services.AddSofisoft();

            // Assert
            Assert.Contains(services, service => service.ServiceType == typeof(SofisoftConfiguration));
        }
    }
}