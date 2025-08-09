using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToteHedger.Core.Models;

namespace ToteHedger.Core.Clients;

public interface ICitibetClient
{
    Task AuthenticateAsync(ApiCredentials credentials, CancellationToken cancellationToken);

    Task<IReadOnlyList<MarketPrice>> GetMarketPricesAsync(string meetingId, CancellationToken cancellationToken);

    Task<BetResponse> PlaceBetAsync(BetRequest betRequest, CancellationToken cancellationToken);
}


