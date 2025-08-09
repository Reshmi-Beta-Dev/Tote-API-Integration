using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToteHedger.Core.Models;

namespace ToteHedger.Core.Clients.Stubs;

public sealed class GlobalToteClientStub : IGlobalToteClient
{
    private bool _authed;

    public Task AuthenticateAsync(ApiCredentials credentials, CancellationToken cancellationToken)
    {
        _authed = true;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<MarketPrice>> GetMarketPricesAsync(string meetingId, CancellationToken cancellationToken)
    {
        var rnd = Random.Shared;
        var prices = new List<MarketPrice>
        {
            new() { MarketId = "M1", SelectionId = "S1", Price = Math.Round((decimal)rnd.NextDouble() * 10m, 2), Available = 100 },
            new() { MarketId = "M1", SelectionId = "S2", Price = Math.Round((decimal)rnd.NextDouble() * 10m, 2), Available = 100 },
            new() { MarketId = "M2", SelectionId = "S1", Price = Math.Round((decimal)rnd.NextDouble() * 10m, 2), Available = 100 }
        };
        return Task.FromResult((IReadOnlyList<MarketPrice>)prices);
    }

    public Task<BetResponse> PlaceBetAsync(BetRequest betRequest, CancellationToken cancellationToken)
    {
        if (!_authed) throw new InvalidOperationException("Not authenticated");
        return Task.FromResult(new BetResponse { Provider = "GlobalTote", BetId = Guid.NewGuid().ToString("N"), Status = "Accepted" });
    }
}


