using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToteHedger.Core.Clients;
using ToteHedger.Core.Clients.Stubs;
using ToteHedger.Core.Hedging;
using ToteHedger.Core.Models;

namespace ToteHedger.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Config
                services.Configure<AppConfig>(context.Configuration);

                // API Clients: use stubs until real implementations are provided
                services.AddSingleton<IGlobalToteClient, GlobalToteClientStub>();
                services.AddSingleton<ICitibetClient, CitibetClientStub>();

                // Hedging
                services.AddSingleton<IHedgingService, HedgingService>();

                // ViewModels
                services.AddSingleton<MainViewModel>();

                // Views
                services.AddSingleton<MainWindow>(sp =>
                {
                    var window = new MainWindow
                    {
                        DataContext = sp.GetRequiredService<MainViewModel>()
                    };
                    return window;
                });
            })
            .Build();

        _host.Start();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
        base.OnExit(e);
    }
}

