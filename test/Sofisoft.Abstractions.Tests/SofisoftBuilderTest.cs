using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Sofisoft.Abstractions.Tests
{
    public class SofisoftBuilderTest
    {
        [Fact]
        public void Constructor_ThrowsAnExceptionForNullServices()
        {
            // Arrange
            var services = (IServiceCollection) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new SofisoftBuilder(services));

            // Assert
            Assert.Equal("services", exception.ParamName);
        }
    }
}