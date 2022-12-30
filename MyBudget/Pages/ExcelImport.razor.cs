using Microsoft.AspNetCore.Components;
using MyBudget.Models;
using MyBudget.Services;
using MudBlazor;
using MyBudget.TempModels;
using MyBudget.Helpers;

namespace MyBudget.Pages
{
	public partial class ExcelImport
	{
		private List<IncomeHistory> incomeHistoryList;
		private List<ExpenseHistory> expenseHistoryList;
		private List<TempIncomeHistory> tempIncomeHistoryList;
		private List<TempExpenseHistory> tempExpenseHistoryList;
		private List<ExpenseCategories> expenseCategoryList;
		private FileResult file;
		private string selectedFileString;
		private readonly string badFile = "badfiletype";
		private bool ShowLoading = false;

		[Inject] IHistoryService<IncomeHistory> IncomeHistoryService { get; set; }
		[Inject] IHistoryService<ExpenseHistory> ExpenseHistoryService { get; set; }
		[Inject] IService<ExpenseCategories> ExpenseCategoryService { get; set; }
		[Inject] ICsvExcelProcessor Processor { get; set; }
		[Inject] ISnackbar Snackbar { get; set; }

		protected override async Task OnInitializedAsync()
		{
			expenseCategoryList = await ExpenseCategoryService.GetList();
			DisplayFile(file);

			tempIncomeHistoryList = new();
			tempExpenseHistoryList = new();
		}

		async Task TestExcel()
		{
			try
			{
				ShowLoading = true;

				var selectedFile = await SelectFile();
				var income_expense_tuple = Processor.ProcessFile(selectedFile);

				tempIncomeHistoryList = income_expense_tuple.Item1;
				tempExpenseHistoryList = income_expense_tuple.Item2;

				ShowLoading = false;
			}
			catch (Exception e)
			{
				ShowLoading = false;
				Snackbar.Add($"{e.Message}", Severity.Error);
			}
		}

		private async Task<FileResult> SelectFile()
		{
			try
			{
				var fileResult = await Processor.Csv_Xslx_Selector();

				if (!(fileResult.FileName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase) ||
					fileResult.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase)))
				{
					fileResult = new FileResult(badFile);
				}
				
				return fileResult;
			}
			catch (Exception e)
			{
				Snackbar.Add($"Error: {e.Message}", Severity.Error);
				return new FileResult(badFile);
			}
		}

		private void ClearFile()
		{
			ClearIncomes();
			ClearExpenses();
			file = null;
			DisplayFile(file);
		}

		private void ClearIncomes()
		{
			ClearList(tempIncomeHistoryList);
		}

		private void ClearExpenses()
		{
			ClearList(tempExpenseHistoryList);
		}

		private static void ClearList<T>(List<T> list)
		{
			list?.Clear();
		}

		private void DisplayFile(FileResult file)
		{
			if (file != null)
			{
				selectedFileString = file.FileName;
			}
			else
			{
				selectedFileString = "no file selected";
			}
		}
	}
}
