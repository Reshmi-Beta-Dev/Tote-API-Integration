namespace ToteHedger.Core.Models;

public sealed class BetResponse
{
    public string Provider { get; init; } = string.Empty;
    public string BetId { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? Message { get; init; }
}


