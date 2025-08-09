using System.Threading;
using System.Threading.Tasks;

namespace ToteHedger.Core.Hedging;

public interface IHedgingService
{
    Task StartAsync(string meetingId, CancellationToken cancellationToken);
    Task StopAsync();
    bool IsRunning { get; }
}


