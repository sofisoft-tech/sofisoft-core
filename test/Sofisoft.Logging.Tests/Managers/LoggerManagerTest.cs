using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Sofisoft.Logging;
using Sofisoft.Logging.Managers;
using Xunit;

namespace Sofisoft.Logging.Tests.Managers
{
    public class LoggerManagerTest
    {

        private readonly Mock<ILogger<LoggerManager>> _loggerMock;
        private readonly Mock<IOptionsMonitor<SofisoftLoggingOptions>> _optionsMock;

        public LoggerManagerTest()
        {
            _loggerMock = new Mock<ILogger<LoggerManager>>();
            _optionsMock = new Mock<IOptionsMonitor<SofisoftLoggingOptions>>();
        }

        [Fact]
        public void Constructor_receivenull_logger_throws()
        {
            Assert.Throws<ArgumentNullException>(() => new LoggerManager((ILogger<LoggerManager>?) null, _optionsMock.Object));
        }

        [Fact]
        public void Constructor_receivenull_options_throws()
        {
            Assert.Throws<ArgumentNullException>(() => new LoggerManager(_loggerMock.Object, (IOptionsMonitor<SofisoftLoggingOptions>?) null));
        }

        [Theory]
        [InlineData("trace", null, null)]
        [InlineData(null, "userAgent", null)]
        [InlineData(null, null, "username")]
        [InlineData("trace", "userAgent", null)]
        [InlineData(null, "userAgent", "username")]
        [InlineData("trace", null, "username")]
        [InlineData("trace", "userAgent", "username")]
        public async Task Error_async_return_guid_string(string trace, string userAgent, string username)
        {
            // Arrange
            var expectedResult = Guid.NewGuid().ToString();
            var handlerMock = new Mock<HttpMessageHandler>();

            _optionsMock.SetupGet(x => x.CurrentValue)
                .Returns(new SofisoftLoggingOptions());

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new StringContent(expectedResult),
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            // Act
            var loggerManager = new LoggerManager(_loggerMock.Object, _optionsMock.Object);

            loggerManager.HttpClient = httpClient;

            var result = await loggerManager.ErrorAsync(It.IsAny<string>(), trace, username, userAgent);

            // Assert
            Guid guidResult;
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.True(Guid.TryParse(result, out guidResult));
        }

        [Fact]
        public async Task Error_async_when_badrequest_return_null()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            _optionsMock.SetupGet(x => x.CurrentValue)
                .Returns(new SofisoftLoggingOptions());

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            // Act
            var loggerManager = new LoggerManager(_loggerMock.Object, _optionsMock.Object);

            loggerManager.HttpClient = httpClient;

            var result = await loggerManager.ErrorAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Error_async_when_exception_return_null()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            _optionsMock.SetupGet(x => x.CurrentValue)
                .Returns(new SofisoftLoggingOptions());

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(It.IsAny<Exception>())
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            // Act
            var loggerManager = new LoggerManager(_loggerMock.Object, _optionsMock.Object);

            loggerManager.HttpClient = httpClient;

            var result = await loggerManager.ErrorAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Information_async_return_guid_string()
        {
            // Arrange
            var expectedResult = Guid.NewGuid().ToString();
            var handlerMock = new Mock<HttpMessageHandler>();

            _optionsMock.SetupGet(x => x.CurrentValue)
                .Returns(new SofisoftLoggingOptions());

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new StringContent(expectedResult),
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            // Act
            var loggerManager = new LoggerManager(_loggerMock.Object, _optionsMock.Object);

            loggerManager.HttpClient = httpClient;

            var result = await loggerManager.InformationAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Guid guidResult;
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.True(Guid.TryParse(result, out guidResult));
        }

        [Fact]
        public async Task Warning_async_return_guid_string()
        {
            // Arrange
            var expectedResult = Guid.NewGuid().ToString();
            var handlerMock = new Mock<HttpMessageHandler>();

            _optionsMock.SetupGet(x => x.CurrentValue)
                .Returns(new SofisoftLoggingOptions());

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new StringContent(expectedResult),
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:3000")
            };

            // Act
            var loggerManager = new LoggerManager(_loggerMock.Object, _optionsMock.Object);

            loggerManager.HttpClient = httpClient;

            var result = await loggerManager.WarningAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Guid guidResult;
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.True(Guid.TryParse(result, out guidResult));
        }
    
        [Fact]
        public void GetHttpClient_return_not_null()
        {
            //Arrange
            Type type = typeof(LoggerManager);
            var loggerManager = Activator.CreateInstance(type, new object[] { _loggerMock.Object, _optionsMock.Object } );

            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "GetHttpClient" && x.IsPrivate)
                .First();

            _optionsMock.SetupGet(x => x.CurrentValue)
                .Returns(new SofisoftLoggingOptions());

            //Act
            var httpClient = (HttpClient?) method.Invoke(loggerManager, Array.Empty<object>());

            //Assert
            Assert.NotNull(httpClient);
        }
    }
}