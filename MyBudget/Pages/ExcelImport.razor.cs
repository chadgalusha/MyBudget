using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyBudget.Models;
using MyBudget.Services;
using ExcelDataReader;
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
		//private IBrowserFile file;
		private FileResult file;
		private string selectedFileString;
		private readonly string badFile = "badfiletype";

		[Inject] IHistoryService<IncomeHistory> IncomeHistoryService { get; set; }
		[Inject] IHistoryService<ExpenseHistory> ExpenseHistoryService { get; set; }
		[Inject] IService<ExpenseCategories> ExpenseCategoryService { get; set; }
		[Inject] ISnackbar Snackbar { get; set; }

		protected override async Task OnInitializedAsync()
		{
			expenseCategoryList = await ExpenseCategoryService.GetList();
			ShowSelectedFile(file);
		}

		private async Task<FileResult> SelectFile()
		{
			try
			{
				//var result = await FilePicker.Default.PickAsync();
				CsvExcelProcessor c = new();
				var result = await c.Csv_Xslx_Selector();

				if (!(result.FileName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase) ||
					result.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase)))
				{
					result = new FileResult(badFile);
				}

				return result;
			}
			catch (Exception e)
			{
				Snackbar.Add($"Error: {e.Message}", Severity.Error);
				return new FileResult(badFile);
			}
		}

		private void ClearFile()
		{
			file = null;
			ShowSelectedFile(file);
			ClearList(tempIncomeHistoryList);
			ClearList(tempExpenseHistoryList);
		}

		private void ClearList<T>(List<T> list)
		{
			if (list != null)
			{
				list.Clear();
			}
		}

		private void ShowSelectedFile(FileResult file)
		{
			if (file != null)
			{
				selectedFileString = file.FullPath;
			}
			else
			{
				selectedFileString = "no file selected";
			}
		}
	}
}
