namespace ToteHedger.Core.Models;

public sealed class MarketPrice
{
    public string MarketId { get; init; } = string.Empty;
    public string SelectionId { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal Available { get; init; }
}


