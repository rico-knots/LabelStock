using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LabelStock.Core;
using LabelStock.Core.Abstractions;
using LabelStock.Core.Data;
using LabelStock.Desktop.Demo;
using LabelStock.Desktop.Services;
using LabelStock.Desktop.ViewModels;
using LabelStock.Desktop.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LabelStock.Desktop;

public partial class App : Application
{
    private ServiceProvider? _provider;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _provider = ConfigureServices();
            
            using (var db = _provider.GetRequiredService<IDbContextFactory<LabelStockDbContext>>().CreateDbContext())
            {
	            db.Database.Migrate();
            }

            // Start on the scan screen.
            _provider.GetRequiredService<Navigator>().GoTo<ScanViewModel>();

            desktop.MainWindow = new MainWindow
            {
                DataContext = _provider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // ---- Data and logic ----
        // These are in-memory demo versions so the screens work before the database exists.
        // When the real services exist in LabelStock.Core, replace this block with services.AddCore().
        services.AddSingleton<DemoStore>();
        /*services.AddSingleton<ILabelService, DemoLabelService>();*/
        /*services.AddSingleton<IStockService, DemoStockService>();*/
        /*services.AddSingleton<IAuthService, DemoAuthService>();*/
        services.AddSingleton<IBackupService, DemoBackupService>();
        
        services.AddCoreServices();

        // ---- UI ----
        services.AddSingleton<Navigator>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<ScanViewModel>();
        services.AddTransient<LabelDetailViewModel>();
        services.AddTransient<NewLabelViewModel>();
        services.AddTransient<AdminLoginViewModel>();
        services.AddTransient<SetAdminPasswordViewModel>();
        services.AddTransient<AdminViewModel>();

        return services.BuildServiceProvider();
    }
}
