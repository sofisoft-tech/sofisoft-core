using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sofisoft.Abstractions.Managers;

namespace Sofisoft.Logging.Managers
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger<LoggerManager> _logger;
        private readonly IOptionsMonitor<SofisoftLoggingOptions> _options;
        public HttpClient? HttpClient { get; set; } 

        public LoggerManager(ILogger<LoggerManager>? logger, IOptionsMonitor<SofisoftLoggingOptions>? options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<string?> ErrorAsync(string message, string trace, string username) =>
            ErrorAsync(message, trace, username, string.Empty);

        public Task<string?> ErrorAsync(string message, string trace, string username, string userAgent)
            => LoggerAsync(_options.CurrentValue.ErrorEndPointUri, message, string.Empty, username, userAgent);

        public Task<string?> InformationAsync(string message, string username)
            => InformationAsync(message, username, string.Empty);

        public Task<string?> InformationAsync(string message, string username, string userAgent)
            => LoggerAsync(_options.CurrentValue.InformationEndPointUri, message, string.Empty, username, userAgent);

        public Task<string?> WarningAsync(string message, string username)
            => WarningAsync(message, username, string.Empty);

        public Task<string?> WarningAsync(string message, string username, string userAgent)
            => LoggerAsync(_options.CurrentValue.WarningEndPointUri, message, string.Empty, username, userAgent);

        private async Task<string?> LoggerAsync(string endPoint, string message, string trace, string username, string userAgent)
        {
            try
            {
                using (var client = this.GetHttpClient())
                {
                    var dto = new {
                        source = _options.CurrentValue.Source, 
                        message, 
                        trace = string.IsNullOrEmpty(trace) ? null : trace,
                        userAgent = string.IsNullOrEmpty(userAgent) ? null : userAgent,
                        username = string.IsNullOrEmpty(username) ? null : username
                    };
                    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(endPoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        _logger.LogWarning("LoggingAPI status: {Status}", response.StatusCode);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Logging service failed: {Message}", ex.Message);
                
                return null;
            }
        }

        private HttpClient GetHttpClient()
        {
            if (HttpClient is null)
            {
                var _httpClient = new HttpClient();
                
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.BaseAddress = _options.CurrentValue.BaseAddress;
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_options.CurrentValue.TokenType,
                        _options.CurrentValue.TokenValue);
                
                return _httpClient;  
            }
            
            return HttpClient;
        }
    }
}