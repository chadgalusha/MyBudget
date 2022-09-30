using Microsoft.Extensions.DependencyInjection;
using MyBudget.DataAccess;
using MyBudget.Services;
using Serilog;

namespace MyBudget;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File(@"C:\Users\ChadGalusha\source\repos\MyBudget\MyBudget\Data\logs.txt",
				fileSizeLimitBytes: 5_000_000,
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
        builder.Services.AddTransient<IncomeTypeService>();
        builder.Services.AddTransient<ExpenseTypeService>();

		// DataAccess Transients
		builder.Services.AddTransient<PaymentFrequencyTypeDataAccess>();
        builder.Services.AddTransient<BankAccountTypeDataAccess>();
        builder.Services.AddTransient<IncomeTypeDataAccess>();
        builder.Services.AddTransient<ExpenseTypeDataAccess>();
        builder.Services.AddTransient<ExpenseDataAccess>();

		// dependency injection
		builder.Services.AddScoped<IIncomeService, IncomeService>();
		builder.Services.AddScoped<IIncomeDataAccess, IncomeDataAccess>();

        return builder.Build();
	}
}
