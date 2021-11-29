using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using Sofisoft.Abstractions;
using Sofisoft.Abstractions.Exceptions;
using Sofisoft.Abstractions.Managers;
using Xunit;

namespace Sofisoft.Logging.Tests
{
    public class SofisoftLoggingMiddlewareTest
    {
        private readonly Mock<ILoggerManager> _loggerManagerMock;

        public SofisoftLoggingMiddlewareTest()
        {
            _loggerManagerMock = new Mock<ILoggerManager>();
        }

        [Fact]
        public void Constructor_ThrowsAnExceptionForNullLogger()
        {
            // Arrange
            var logger = (ILoggerManager) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new SofisoftLoggingMiddleware(logger));

            // Assert
            Assert.Equal("logger", exception.ParamName);
        }

        [Theory]
        [InlineData(typeof(FakeBadRequestException), "error message", 400, "DELETE", null, "admin")]
        [InlineData(typeof(OperationCanceledException), "error message", 400, "GET", null, null)]
        [InlineData(typeof(TaskCanceledException), "error message", 400, "GET", null, null)]
        [InlineData(typeof(FakeNotFoundException), "error message", 404, "PATCH", null, null)]
        [InlineData(typeof(ValidationException), "error message", 422, "POST", null, null)]
        [InlineData(typeof(Exception), null, 500, "DELETE", null, null)]
        [InlineData(typeof(Exception), null, 500, "GET", null, null)]
        [InlineData(typeof(Exception), null, 500, "OPTIONS", null, null)]
        [InlineData(typeof(Exception), null, 500, "PATCH", null, null)]
        [InlineData(typeof(Exception), null, 500, "POST", null, null)]
        [InlineData(typeof(Exception), null, 500, "PUT", null, null)]
        [InlineData(typeof(DivideByZeroException), "error message", 500, "GET", "guid", null)]
        public async Task InvokeAsync_throws_badrequest_exception(Type type, string error, int statusCode, string method, string guid, string user)
        {
            // Arrange
            var errors = new Dictionary<string, string[]>();
            var httpContextMock = new Mock<HttpContext>();
            var exception = (Exception) Activator.CreateInstance(type, statusCode == 422 ? new object[] { errors } : new object[] { error } )!;
            var responseMock = new Mock<HttpResponse>();
            var identityMock = new Mock<IIdentity>();

            RequestDelegate nextMiddleware = (HttpContext) =>
            {
                return Task.FromException(exception);
            };

            var headers = new Dictionary<string, StringValues>()
            {
                { HeaderNames.UserAgent, "Mozilla/5.0" }
            };

            _loggerManagerMock.Setup(x => x.ErrorAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(guid);
            httpContextMock.Setup(x => x.Request.Headers).Returns(new HeaderDictionary(headers));
            httpContextMock.Setup(x => x.Response).Returns(responseMock.Object);
            httpContextMock.Setup(x => x.Response.ContentType).Returns("application/json");
            httpContextMock.Setup(x => x.Response.StatusCode).Returns(statusCode);
            httpContextMock.Setup(x => x.User).Returns(new ClaimsPrincipal());
            httpContextMock.Setup(x => x.User.Identity).Returns(identityMock.Object);
            httpContextMock.Setup(x => x.User.Identity!.Name).Returns(user);
            httpContextMock.Setup(x => x.Request.Method).Returns(method);
            httpContextMock.Setup(x => x.RequestServices).Returns(GetServiceProvider());

            responseMock.Setup(x => x.Body.WriteAsync(It.IsAny<byte[]>(),It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Callback((byte[] data, int offset, int length, CancellationToken token)=> {});
            
            var httpContext = httpContextMock.Object;

            // Act
            var middleware = new SofisoftLoggingMiddleware(_loggerManagerMock.Object);
            await middleware.InvokeAsync(httpContext, nextMiddleware);

            // Assert
            Assert.Equal("application/json", httpContext.Response.ContentType);
            Assert.Equal(statusCode, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_success()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            RequestDelegate nextMiddleware = (HttpContext) =>
            {
                return Task.CompletedTask;
            };

            httpContextMock.Setup(x => x.Response).Returns(responseMock.Object);
            httpContextMock.Setup(x => x.Response.ContentType).Returns("application/json");
            httpContextMock.Setup(x => x.Response.StatusCode).Returns(200);

            var httpContext = httpContextMock.Object;

            // Act
            var middleware = new SofisoftLoggingMiddleware(_loggerManagerMock.Object);
            await middleware.InvokeAsync(httpContext, nextMiddleware);

            // Assert
            Assert.Equal("application/json", httpContext.Response.ContentType);
            Assert.Equal(200, httpContext.Response.StatusCode);
        }

        private static ServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.TryAddScoped<SofisoftConfiguration>();

            return services.BuildServiceProvider();
        }

        class FakeBadRequestException : BadRequestException
        {
            public FakeBadRequestException(string message) : base(message){ }
        }

        class FakeNotFoundException : NotFoundException
        {
            public FakeNotFoundException(string message) : base(message) { }
        }
    }
}