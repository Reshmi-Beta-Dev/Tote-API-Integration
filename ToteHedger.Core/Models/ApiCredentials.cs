namespace ToteHedger.Core.Models;

public sealed class ApiCredentials
{
    public string BaseUrl { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? ApiKey { get; init; }
}


