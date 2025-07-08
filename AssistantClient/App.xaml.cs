using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AssistantClient.Services;
using AssistantClient.ViewModels;
using System.Windows;
using System.IO;

namespace AssistantClient;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton<IPatientService, PatientService>();
        services.AddTransient<PatientViewModel>();

        Services = services.BuildServiceProvider();

        var mainWindow = new MainWindow
        {
            DataContext = Services.GetRequiredService<PatientViewModel>()
        };
    }
}
