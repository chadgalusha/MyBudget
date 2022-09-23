using Microsoft.AspNetCore.Components.WebView.Maui;
using MyBudget.DataAccess;
using MyBudget.Models;
using MyBudget.Services;
using Serilog;

namespace MyBudget;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File(@"C:\Users\ChadGalusha\source\repos\BudgetApplication\BudgetApplication\AppData\logs.txt",
				fileSizeLimitBytes: 1000,
				retainedFileCountLimit: 1,
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
			.CreateLogger();

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("materialdesignicons-webfont.ttf", "IconFontTypes");
            });

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		// Service Transients
		builder.Services.AddTransient<PaymentFrequencyTypeService>();
		builder.Services.AddTransient<BankAccountTypeService>();

		// DataAccess Transients
		builder.Services.AddTransient<BankAccountTypeDataAccess>();

        return builder.Build();
	}
}
