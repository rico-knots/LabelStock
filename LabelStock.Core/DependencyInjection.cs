using LabelStock.Core.Abstractions;
using LabelStock.Core.Data;
using LabelStock.Core.Services;
using LabelStock.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LabelStock.Core;

public static class DependencyInjection
{
    public static void AddCoreServices(this IServiceCollection services)
    {
	    var settings = new AppSettings();
	    
        services.AddSingleton(settings);
        services.AddDbContextFactory<LabelStockDbContext>(options =>
	        options.UseSqlite($"Data Source={settings.DatabaseFile}"));
        
        services.AddSingleton<ILabelService, LabelService>();
        services.AddSingleton<IStockService, StockService>();
        services.AddSingleton<IAuthService, AuthService>();
    }
}