using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Sofisoft.MongoDb.Tests
{
    public class SofisoftMongoDbExtensionsTest
    {
        [Fact]
        public void AddMongoDb_throws_an_exception_for_null_builder()
        {
            // Arrange
            var builder = (SofisoftBuilder) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddMongoDb());

            // Assert
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddMongoDb_throws_an_exception_for_null_builder_in_configuration()
        {
            // Arrange
            var builder = (SofisoftBuilder) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddMongoDb(configuration: null!));

            // Assert
            Assert.Equal("builder", exception.ParamName);

        }

        [Fact]
        public void AddMongoDb_throws_an_exception_for_null_configuration()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => builder.AddMongoDb(configuration: null!));

            // Assert
            Assert.Equal("configuration", exception.ParamName);

        }

        [Fact]
        public void AddMongoDb_set_options()
        {
            // Arrange
            var services = CreateServices();
            var builder = CreateBuilder(services);

            // Act
            builder.AddMongoDb(options =>
            {
                options.SetConnectionString("my-connection-string");
                options.SetDatabase("my-database");
            });

            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptionsMonitor<SofisoftMongoDbOptions>>();

            // Assert
            Assert.Equal("my-connection-string", options.CurrentValue.ConnectionString);
            Assert.Equal("my-database", options.CurrentValue.Database);
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