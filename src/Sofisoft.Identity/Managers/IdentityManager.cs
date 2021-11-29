using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sofisoft.Abstractions.Managers;

namespace Sofisoft.Identity.Managers
{
    public sealed class IdentityManager : IIdentityManager
    {
        private readonly IHttpContextAccessor _context;
        private readonly IOptionsMonitor<SofisoftIdentityOptions> _options;

        public IdentityManager(IHttpContextAccessor context, IOptionsMonitor<SofisoftIdentityOptions> options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public string? ClientAppId
            => _context.HttpContext.User.FindFirst(_options.CurrentValue.ClientIdKey)?.Value;

        public string? CompanyId
            => _context.HttpContext.User.FindFirst(_options.CurrentValue.CompanyIdKey)?.Value;

        public string Token {
            get {
                var strings = _context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Split(" ");

                if(strings.Length < 2)
                {
                    return string.Empty;
                }

                return strings[1];
            }
        }

        public string TokenType
            => _context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[0];

        public string? UserId
            => _context.HttpContext.User.FindFirst(_options.CurrentValue.UserIdKey)?.Value;

        public string? Username
            => _context.HttpContext.User.FindFirst(_options.CurrentValue.UsernameKey)?.Value;
    }
}