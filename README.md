### ToteHedger

WPF (.NET 8) application showcasing integration scaffolding for Global Tote and Citibet with an automated hedging loop. The current implementation uses stub clients; swap them for real API clients once credentials and docs are provided.

### Prerequisites
- .NET SDK 8.0+
- Windows 10/11 (WPF)

### Solution layout
```
ToteHedger.sln
├─ ToteHedger.App/        # WPF UI + DI/hosting + appsettings
│  ├─ App.xaml, App.xaml.cs
│  ├─ MainWindow.xaml(.cs)
│  ├─ MainViewModel.cs
│  ├─ AppConfig.cs
│  └─ appsettings.json    # placeholders; safe to commit
└─ ToteHedger.Core/       # Core contracts and hedging logic
   ├─ Models/             # ApiCredentials, MarketPrice, BetRequest, BetResponse
   ├─ Clients/            # IGlobalToteClient, ICitibetClient
   │  └─ Stubs/           # Stub implementations until real APIs provided
   └─ Hedging/            # IHedgingService, HedgingService
```

### Configure
Edit `ToteHedger.App/appsettings.json`:
```json
{
  "MeetingId": "M1",
  "GlobalTote": { "BaseUrl": "https://...", "Username": "", "Password": "", "ApiKey": "" },
  "Citibet":    { "BaseUrl": "https://...", "Username": "", "Password": "", "ApiKey": "" }
}
```
For secrets, prefer a local `appsettings.Local.json` (ignored) or User Secrets.

### Build & run
```powershell
# from repo root
dotnet build
dotnet run --project ToteHedger.App/ToteHedger.App.csproj
```
If you see a run error, run a build first, then run again.

### Replacing stubs with real APIs
- Implement `IGlobalToteClient` and `ICitibetClient` using `HttpClient` and your API schemas.
- Register them in `App.xaml.cs` instead of the stubs:
```csharp
services.AddHttpClient<IGlobalToteClient, GlobalToteHttpClient>();
services.AddHttpClient<ICitibetClient, CitibetHttpClient>();
```
- Map auth and endpoints using `ApiCredentials` and your request/response models.

### Hedging logic
`HedgingService` polls prices from both venues and places a tiny illustrative hedge when a price delta threshold is exceeded. Replace the placeholder logic with your production rules.

### License
Private/internal demo unless specified otherwise.


