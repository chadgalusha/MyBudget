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

		async Task SelectFile()
		{
			try
			{
				ShowLoadingOn();

				var selectedFile = await Processor.Csv_Xslx_Selector();
				var income_expense_tuple = Processor.ProcessFile(selectedFile);

				SetTempLists(income_expense_tuple);

				DisplayFile(selectedFile);

				ShowLoadingOff();
			}
			catch (Exception e)
			{
				ShowLoadingOff();
				Snackbar.Add($"Error with file, or improperly formatted file", Severity.Error);
				MyBudgetLogger.ErrorLogMessage(e);
			}
		}

		private void SetTempLists((List<TempIncomeHistory>, List<TempExpenseHistory>) income_expense_tuple)
		{
			tempIncomeHistoryList = income_expense_tuple.Item1;
			tempExpenseHistoryList = income_expense_tuple.Item2;
		}

		private async Task SaveIncomes()
		{
			try
			{
				int incomesProcessed = 0;

				foreach (var tempIncome in tempIncomeHistoryList)
				{
					IncomeHistory newIncome = GetNewIncomeHistory(tempIncome);

					var result = await IncomeHistoryService.CreateRecord(newIncome);

					if (result.IncomeHistoryId != 0)
					{
						incomesProcessed++;
					}
				}

				Snackbar.Add($"Expected Incomes to save: [{tempIncomeHistoryList.Count}]. Actual Incomes saved: [{incomesProcessed}]", Severity.Success);
				ClearIncomes();
			}
			catch (Exception e)
			{
				Snackbar.Add($"Error processing Incomes: {e.Message}", Severity.Error);
			}
		}

		private async Task SaveExpenses()
		{
			try
			{
				int expensesProcessed = 0;

				foreach (var tempExpense in tempExpenseHistoryList)
				{
					ExpenseHistory newExpense = GetNewExpenseHistory(tempExpense);

					var result = await ExpenseHistoryService.CreateRecord(newExpense);

					if (result.ExpenseHistoryId != 0)
					{
						expensesProcessed++;
					}
				}

				Snackbar.Add($"Expected Expenses to save: [{tempExpenseHistoryList.Count}]. Actual Expenses saved: [{expensesProcessed}]", Severity.Success);
				ClearExpenses();
			}
			catch (Exception e)
			{
				Snackbar.Add($"Error processing Expenses: {e.Message}", Severity.Error);
			}
		}

		private IncomeHistory GetNewIncomeHistory(TempIncomeHistory tempIncomeHistory)
		{
			return new()
			{
				IncomeName	 = tempIncomeHistory.IncomeName,
				IncomeDate	 = (DateTime)tempIncomeHistory.IncomeDate,
				IncomeAmount = tempIncomeHistory.IncomeAmount
			};
		}

		private ExpenseHistory GetNewExpenseHistory(TempExpenseHistory tempExpenseHistory)
		{
			return new()
			{
				ExpenseName		  = tempExpenseHistory.ExpenseName,
				ExpenseDate		  = (DateTime)tempExpenseHistory.ExpenseDate,
				AmountPaid		  = tempExpenseHistory.AmountPaid,
				ExpenseCategoryId = tempExpenseHistory.ExpenseCategoryId
			};
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
				selectedFileString = file.FullPath;
			}
			else
			{
				selectedFileString = "no file selected";
			}
		}

		private void ShowLoadingOn()
		{
			ShowLoading = true;
		}

		private void ShowLoadingOff()
		{
			ShowLoading = false;
		}
	}
}
