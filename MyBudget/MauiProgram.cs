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
		// Service Transients
        //builder.Services.AddTransient<ExpenseTypeService>();

		// DataAccess Transients
        //builder.Services.AddTransient<ExpenseTypeDataAccess>();

        // dependency injection data access
        builder.Services.AddScoped<IDataAccess<BankAccountTypes>, BankAccountTypeDataAccess>();
        builder.Services.AddScoped<IExpenseTypeDataAccess, ExpenseTypeDataAccess>();
        builder.Services.AddScoped<IIncomeDataAccess, IncomeDataAccess>();
		builder.Services.AddScoped<IIncomeTypeDataAccess, IncomeTypeDataAccess>();
		builder.Services.AddScoped<IDataAccess<PaymentFrequencyTypes>, PaymentFrequencyTypeDataAccess>();
		builder.Services.AddScoped<IExpenseDataAccess, ExpenseDataAccess>();
		
        // dependency injection services
        builder.Services.AddScoped<IBankAccountTypeService, BankAccountTypeService>();
        builder.Services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
        builder.Services.AddScoped<IIncomeService, IncomeService>();
        builder.Services.AddScoped<IIncomeTypeService, IncomeTypeService>();
        builder.Services.AddScoped<IPaymentFrequencyTypeService, PaymentFrequencyTypeService>();
		builder.Services.AddScoped<IExpenseService, ExpenseService>();

        return builder.Build();
	}
}
