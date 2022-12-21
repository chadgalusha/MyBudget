using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyBudget.Models;
using MyBudget.Services;
using ExcelDataReader;
using MudBlazor;

namespace MyBudget.Pages
{
	public partial class ExcelImport
	{
		private List<IncomeHistory> incomeHistoryList;
		private List<ExpenseHistory> expenseHistoryList;
		private List<ExpenseCategories> expenseCategoryList;
		private IBrowserFile file;
		private readonly string badFile = "badfiletype";

		[Inject] IHistoryService<IncomeHistory> IncomeHistoryService { get; set; }
		[Inject] IHistoryService<ExpenseHistory> ExpenseHistoryService { get; set; }
		[Inject] IService<ExpenseCategories> ExpenseCategoryService { get; set; }
		[Inject] ISnackbar Snackbar { get; set; }

		protected override async Task OnInitializedAsync()
		{
			expenseCategoryList = await ExpenseCategoryService.GetList();
		}

		private async Task<FileResult> SelectFile()
		{
			try
			{
				var result = await FilePicker.Default.PickAsync();

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
	}
}
