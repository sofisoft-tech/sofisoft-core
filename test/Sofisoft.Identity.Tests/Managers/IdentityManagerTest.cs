using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using Sofisoft.Identity.Managers;
using Xunit;

namespace Sofisoft.Identity.Tests.Managers
{
    public class IdentityManagerTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextMock;
        private readonly Mock<IOptionsMonitor<SofisoftIdentityOptions>> _optionsMock;

        public IdentityManagerTest()
        {
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _optionsMock = new Mock<IOptionsMonitor<SofisoftIdentityOptions>>();
            _optionsMock.Setup(x => x.CurrentValue).Returns(new SofisoftIdentityOptions());
        }

        [Fact]
        public void Constructor_receive_context_null()
        {
            // Arrange
            var context = (IHttpContextAccessor) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new IdentityManager(context, _optionsMock.Object));

            // Assert
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void Constructor_receive_options_null()
        {
            // Arrange
            var options = (IOptionsMonitor<SofisoftIdentityOptions>) null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => new IdentityManager(_httpContextMock.Object, options));

            // Assert
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public void ClientAppId_return_value()
        {
            // Arrange
            var expectedResult = "my-app-id";
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();
            var claims = new List<Claim> { new Claim(_optionsMock.Object.CurrentValue.ClientIdKey, expectedResult) };
            var identity = new ClaimsIdentity(claims);

            principal.AddIdentity(identity);
            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.ClientAppId;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ClientAppId_return_null_when_not_claim()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.ClientAppId;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CompanyId_return_value()
        {
            // Arrange
            var expectedResult = "my-company-id";
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();
            var claims = new List<Claim> { new Claim(_optionsMock.Object.CurrentValue.CompanyIdKey, expectedResult) };
            var identity = new ClaimsIdentity(claims);

            principal.AddIdentity(identity);
            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.CompanyId;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CompanyId_return_null_when_not_claim()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.CompanyId;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Token_return_value()
        {
            // Arrange
            var expectedResult = "dGVzdA==";
            var httpContextMock = new Mock<HttpContext>();

            var headers = new Dictionary<string, StringValues>()
            {
                { HeaderNames.Authorization, $"Basic {expectedResult}" }
            };

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            _httpContextMock.Setup(x => x.HttpContext.Request.Headers).Returns(new HeaderDictionary(headers));

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.Token;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Token_not_header_authorization_return_empty()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var headers = new Dictionary<string, StringValues>();

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            _httpContextMock.Setup(x => x.HttpContext.Request.Headers).Returns(new HeaderDictionary(headers));

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.Token;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void TokenType_return_value()
        {
            // Arrange
            var expectedResult = "Basic";
            var httpContextMock = new Mock<HttpContext>();

            var headers = new Dictionary<string, StringValues>()
            {
                { HeaderNames.Authorization, $"{expectedResult} dGVzdA==" }
            };

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            _httpContextMock.Setup(x => x.HttpContext.Request.Headers).Returns(new HeaderDictionary(headers));

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.TokenType;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UserId_return_value()
        {
            // Arrange
            var expectedResult = "my-user-id";
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();
            var claims = new List<Claim> { new Claim(_optionsMock.Object.CurrentValue.UserIdKey, expectedResult) };
            var identity = new ClaimsIdentity(claims);

            principal.AddIdentity(identity);
            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.UserId;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UserId_return_null_when_not_claim()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.UserId;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UserName_return_value()
        {
            // Arrange
            var expectedResult = "my-username";
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();
            var claims = new List<Claim> { new Claim(_optionsMock.Object.CurrentValue.UsernameKey, expectedResult) };
            var identity = new ClaimsIdentity(claims);

            principal.AddIdentity(identity);
            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.Username;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UserName_return_null_when_not_claim()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var principal = new ClaimsPrincipal();

            _httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(x => x.User).Returns(principal);

            // Act
            var identityManager = new IdentityManager(_httpContextMock.Object, _optionsMock.Object);
            var result = identityManager.Username;

            // Assert
            Assert.Null(result);
        }
    }
}