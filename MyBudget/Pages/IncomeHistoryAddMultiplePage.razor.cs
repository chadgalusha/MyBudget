using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBudget.Models;
using MyBudget.Services;
using MyBudget.TempModels;

namespace MyBudget.Pages
{
	public partial class IncomeHistoryAddMultiplePage
	{
		// Added Temp class due to MudBlazor date picker component needing DateTime? rather than DateTime
		private readonly List<TempIncomeHistory> incomeTempHistoryList = new();

		[Inject] ISnackbar Snackbar { get; set; }
		[Inject] IHistoryService<IncomeHistory> IncomeHistoryService{ get; set; }

		protected override void OnInitialized()
		{
			incomeTempHistoryList.Add(GetInitialIncomeHistory());
		}

		// Table functionality methods

		private void AddRow()
		{
			if (incomeTempHistoryList.Count >= 25)
			{
				Snackbar.Add("Max of 25 new records", Severity.Info);
				return;
			}

			incomeTempHistoryList.Add(GetNextRow());
		}

		private void DeleteRow()
		{
			var lastIndex = incomeTempHistoryList.LastOrDefault();
			incomeTempHistoryList.Remove(lastIndex);
		}

		private TempIncomeHistory GetInitialIncomeHistory()
		{
			return new() { IncomeHistoryId = 1, IncomeDate = DateTime.Now };
		}

		private int NextId()
		{
			return incomeTempHistoryList.Count + 1;
		}

		private TempIncomeHistory GetNextRow()
		{
			return new() { IncomeHistoryId = NextId(), IncomeDate = DateTime.Now };
		}

		// Save methods

		private async Task ProcessList()
		{
			int result = CheckListForIssues();
			if (result != 0)
			{
				Snackbar.Add($"Issue with row {result}. Name must not be empty/blank. Date year must be greater than 1999", Severity.Error);
				return;
			}

			foreach (var income in incomeTempHistoryList)
			{
				IncomeHistory newIncomeHistory = GetNewModel(income);
				await SaveRecord(newIncomeHistory);
			}

			ClearListAndSuccessMessage();
		}

		private void ClearListAndSuccessMessage()
		{
			incomeTempHistoryList.Clear();
			incomeTempHistoryList.Add(GetInitialIncomeHistory());
			Snackbar.Add("New Income Histories Added", Severity.Success);
		}

		private int CheckListForIssues()
		{
			foreach (var income in incomeTempHistoryList)
			{
				if (string.IsNullOrWhiteSpace(income.IncomeName))
				{
					return income.IncomeHistoryId;
				}

				DateTime dateTime = (DateTime)income.IncomeDate;
				if (dateTime.Year < 2000)
				{
					return income.IncomeHistoryId;
				}
			}
			return 0;
		}

		private async Task SaveRecord(IncomeHistory newIncomeHistory)
		{
			try
			{
				await IncomeHistoryService.CreateRecord(newIncomeHistory);
			}
			catch (Exception e)
			{
				Snackbar.Add(e.Message, Severity.Error);
			}
		}

		// Transform TempIncomeHistory to IncomeHistory for saving to db
		private IncomeHistory GetNewModel(TempIncomeHistory income)
		{
			return new()
			{
				IncomeName = income.IncomeName,
				IncomeAmount = income.IncomeAmount,
				IncomeDate = (DateTime)income.IncomeDate
			};
		}
	}
}
