using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToteHedger.Core.Clients;
using ToteHedger.Core.Models;

namespace ToteHedger.Core.Hedging;

public sealed class HedgingService : IHedgingService
{
    private readonly IGlobalToteClient _globalToteClient;
    private readonly ICitibetClient _citibetClient;
    private CancellationTokenSource? _cts;

    public HedgingService(IGlobalToteClient globalToteClient, ICitibetClient citibetClient)
    {
        _globalToteClient = globalToteClient;
        _citibetClient = citibetClient;
    }

    public bool IsRunning => _cts is not null;

    public async Task StartAsync(string meetingId, CancellationToken cancellationToken)
    {
        if (IsRunning) return;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var token = _cts.Token;

        _ = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var globalPrices = await _globalToteClient.GetMarketPricesAsync(meetingId, token);
                    var citibetPrices = await _citibetClient.GetMarketPricesAsync(meetingId, token);

                    foreach (var gp in globalPrices)
                    {
                        var cp = citibetPrices.FirstOrDefault(p => p.MarketId == gp.MarketId && p.SelectionId == gp.SelectionId);
                        if (cp is null) continue;

                        // Placeholder hedging decision: if price difference exceeds threshold, place a bet at better venue
                        var diff = gp.Price - cp.Price;
                        if (Math.Abs(diff) >= 0.1m)
                        {
                            var betterOnGlobal = gp.Price > cp.Price;
                            var targetClient = betterOnGlobal ? _globalToteClient : _citibetClient;

                            var request = new BetRequest
                            {
                                MarketId = gp.MarketId,
                                SelectionId = gp.SelectionId,
                                Stake = 1m,
                                MinPrice = betterOnGlobal ? gp.Price : cp.Price,
                                Reference = $"HEDGE-{DateTimeOffset.UtcNow:yyyyMMddHHmmssfff}"
                            };

                            await targetClient.PlaceBetAsync(request, token);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    // Swallow errors in placeholder loop. In real impl, log and handle.
                }

                await Task.Delay(TimeSpan.FromSeconds(3), token);
            }
        }, token);
    }

    public Task StopAsync()
    {
        _cts?.Cancel();
        _cts = null;
        return Task.CompletedTask;
    }
}


