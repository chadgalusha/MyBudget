using Microsoft.Extensions.DependencyInjection;
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

        // dependency inject type data access
        builder.Services.AddScoped<ITypeDataAccess<IncomeTypes>, IncomeTypeDataAccess>();
		builder.Services.AddScoped<ITypeDataAccess<ExpenseTypes>, ExpenseTypeDataAccess>();
		builder.Services.AddScoped<ITypeDataAccess<PaymentFrequencyTypes>, PaymentFrequencyTypeDataAccess>();
		builder.Services.AddScoped<ITypeDataAccess<BankAccountTypes>, BankAccountTypeDataAccess>();
        // dependency injection data access
        builder.Services.AddScoped<IIncomeDataAccess, IncomeDataAccess>();
		builder.Services.AddScoped<IExpenseDataAccess, ExpenseDataAccess>();
        // dependency inject type service
        builder.Services.AddScoped<ITypeService<IncomeTypes>, IncomeTypeService>();
		builder.Services.AddScoped<ITypeService<ExpenseTypes>, ExpenseTypeService>();
		builder.Services.AddScoped<ITypeService<PaymentFrequencyTypes>, PaymentFrequencyTypeService>();
		builder.Services.AddScoped<ITypeService<BankAccountTypes>, BankAccountTypeService>();
        // dependency injection services
        builder.Services.AddScoped<IIncomeService, IncomeService>();
		builder.Services.AddScoped<IExpenseService, ExpenseService>();

        return builder.Build();
	}
}
