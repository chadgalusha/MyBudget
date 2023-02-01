using MudBlazor;
using MudBlazor.Services;
using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;
using MyBudget.Services;
using Serilog;

namespace MyBudget;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		DatabaseHelper.CheckForDbTables();

		//for reading excel files, otherse 1252 error
		System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

		// dependency inject type data access
		builder.Services.AddTransient<ITypeDataAccess<IncomeTypes>, IncomeTypeDataAccess>();
		builder.Services.AddTransient<ITypeDataAccess<ExpenseTypes>, ExpenseTypeDataAccess>();
		builder.Services.AddTransient<ITypeDataAccess<PaymentFrequencyTypes>, PaymentFrequencyTypeDataAccess>();
		builder.Services.AddTransient<ITypeDataAccess<BankAccountTypes>, BankAccountTypeDataAccess>();
		// dependency injection data access
		builder.Services.AddTransient<IDataAccess<Incomes>, IncomeDataAccess>();
		builder.Services.AddTransient<IDataAccess<Expenses>, ExpenseDataAccess>();
		builder.Services.AddTransient<IDataAccess<BankAccounts>, BankAccountDataAccess>();
		builder.Services.AddTransient<IHistoryDataAccess<IncomeHistory>, IncomeHistoryDataAccess>();
		builder.Services.AddTransient<IHistoryDataAccess<ExpenseHistory>, ExpenseHistoryDataAccess>();
		builder.Services.AddTransient<IDataAccess<ExpenseCategories>, ExpenseCategoriesDataAccess>();
		// dependency inject type service
		builder.Services.AddTransient<ITypeService<IncomeTypes>, IncomeTypeService>();
		builder.Services.AddTransient<ITypeService<ExpenseTypes>, ExpenseTypeService>();
		builder.Services.AddTransient<ITypeService<PaymentFrequencyTypes>, PaymentFrequencyTypeService>();
		builder.Services.AddTransient<ITypeService<BankAccountTypes>, BankAccountTypeService>();
		// dependency injection services
		builder.Services.AddTransient<IService<Incomes>, IncomeService>();
		builder.Services.AddTransient<IService<Expenses>, ExpenseService>();
		builder.Services.AddTransient<IService<BankAccounts>, BankAccountService>();
		builder.Services.AddTransient<IHistoryService<IncomeHistory>, IncomeHistoryService>();
		builder.Services.AddTransient<IHistoryService<ExpenseHistory>, ExpenseHistoryService>();
		builder.Services.AddTransient<IIncomeAndExpensesViewModelService, IncomeAndExpensesViewModelService>();
		builder.Services.AddTransient<IService<ExpenseCategories>, ExpenseCategoriesService>();

		builder.Services.AddTransient<ICalendarProcessor, CalendarProcessor>();
		builder.Services.AddTransient<IChartProcessor, ChartProcessor>();
		builder.Services.AddTransient<ICsvExcelProcessor, CsvExcelProcessor>();

		builder.Services.AddMudServices(config =>
		{
			config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
			config.SnackbarConfiguration.PreventDuplicates = false;
			config.SnackbarConfiguration.NewestOnTop = false;
			config.SnackbarConfiguration.ShowCloseIcon = true;
			config.SnackbarConfiguration.VisibleStateDuration = 10000;
			config.SnackbarConfiguration.HideTransitionDuration = 500;
			config.SnackbarConfiguration.ShowTransitionDuration = 500;
			config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
		});

		return builder.Build();
	}
}
