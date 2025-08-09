using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ToteHedger.Core.Clients;
using ToteHedger.Core.Hedging;
using ToteHedger.Core.Models;

namespace ToteHedger.App;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly IHedgingService _hedgingService;
    private readonly IGlobalToteClient _globalToteClient;
    private readonly ICitibetClient _citibetClient;
    private readonly AppConfig _config;
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private string status = "Idle";

    [ObservableProperty]
    private string meetingId = string.Empty;

    [ObservableProperty]
    private bool isRunning;

    public MainViewModel(IHedgingService hedgingService,
                         IGlobalToteClient globalToteClient,
                         ICitibetClient citibetClient,
                         IOptions<AppConfig> options)
    {
        _hedgingService = hedgingService;
        _globalToteClient = globalToteClient;
        _citibetClient = citibetClient;
        _config = options.Value;

        MeetingId = _config.MeetingId ?? "";
    }

    [RelayCommand]
    private async Task StartAsync()
    {
        if (IsRunning) return;
        Status = "Authenticating...";
        _cts = new CancellationTokenSource();

        var gtCreds = new ApiCredentials
        {
            BaseUrl = _config.GlobalTote.BaseUrl,
            Username = _config.GlobalTote.Username,
            Password = _config.GlobalTote.Password,
            ApiKey = _config.GlobalTote.ApiKey
        };
        var cbCreds = new ApiCredentials
        {
            BaseUrl = _config.Citibet.BaseUrl,
            Username = _config.Citibet.Username,
            Password = _config.Citibet.Password,
            ApiKey = _config.Citibet.ApiKey
        };

        await _globalToteClient.AuthenticateAsync(gtCreds, _cts.Token);
        await _citibetClient.AuthenticateAsync(cbCreds, _cts.Token);

        Status = "Running";
        IsRunning = true;
        await _hedgingService.StartAsync(string.IsNullOrWhiteSpace(MeetingId) ? _config.MeetingId : MeetingId, _cts.Token);
    }

    [RelayCommand]
    private async Task StopAsync()
    {
        if (!IsRunning) return;
        Status = "Stopping...";
        await _hedgingService.StopAsync();
        _cts?.Cancel();
        _cts = null;
        IsRunning = false;
        Status = "Stopped";
    }
}


