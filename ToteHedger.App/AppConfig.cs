namespace ToteHedger.App;

public sealed class AppConfig
{
    public string? MeetingId { get; set; }
    public ApiSection GlobalTote { get; set; } = new();
    public ApiSection Citibet { get; set; } = new();

    public sealed class ApiSection
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? ApiKey { get; set; }
    }
}


