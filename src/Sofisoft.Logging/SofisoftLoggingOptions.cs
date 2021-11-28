namespace Sofisoft.Logging
{
    public class SofisoftLoggingOptions
    {
        public Uri? BaseAddress { get; set; }
        public string ErrorEndPointUri { get; set; } = "api/v1/events/errors";
        public string InformationEndPointUri { get; set; } = "api/v1/events/informations";
        public string? Source { get; set; }
        public string TokenType { get; set; } = "Basic";
        public string? TokenValue { get; set; }
        public string WarningEndPointUri { get; set; } = "api/v1/events/warnings";

    }
}