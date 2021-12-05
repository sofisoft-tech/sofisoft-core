using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Sofisoft.MongoDb.Tests
{
    public class SofisoftMongoDbBuilderTest
    {
        [Fact]
        public void Constructor_ThrowsAnExceptionForNullServices()
        {
            // Arrange
            var services = (IServiceCollection) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new SofisoftMongoDbBuilder(services));

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
        public void SetConnectionString_throws_an_exception_for_null_parameter(string connectionString)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetConnectionString(connectionString));

            // Assert
            Assert.Equal("connectionString", exception.ParamName);
        }

        [Fact]
        public void SetConnectionString_is_set()
        {
            // Arrange
            var expected = "my-connection-string";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetConnectionString(expected);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftMongoDbOptions>>().CurrentValue;

            Assert.Equal(expected, options.ConnectionString);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetDatabase_throws_an_exception_for_null_parameter(string database)
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);
            
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.SetDatabase(database));

            // Assert
            Assert.Equal("database", exception.ParamName);
        }

        [Fact]
        public void SetDatabase_is_set()
        {
            // Arrange
            var expected = "my-database";
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.SetDatabase(expected);

            // Assert
            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftMongoDbOptions>>().CurrentValue;

            Assert.Equal(expected, options.Database);
        }

        private static SofisoftMongoDbBuilder CreateBuilder(IServiceCollection services)
            => services.AddSofisoft().AddMongoDb();

        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();

            return services;
        }

    }
}