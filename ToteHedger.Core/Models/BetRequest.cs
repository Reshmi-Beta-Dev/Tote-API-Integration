namespace ToteHedger.Core.Models;

public sealed class BetRequest
{
    public string MarketId { get; init; } = string.Empty;
    public string SelectionId { get; init; } = string.Empty;
    public decimal Stake { get; init; }
    public decimal? MinPrice { get; init; }
    public string? Reference { get; init; }
}


