﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBudget.Models;
using MyBudget.Services;

namespace MyBudget.Pages
{
	public partial class ExpenseHistoryAddMultiplePage
	{
		private readonly List<TempExpenseHistory> expenseTempHistoryList = new();
		private List<ExpenseCategories> expenseCategoryList;

		[Inject] ISnackbar Snackbar { get; set; }
		[Inject] IHistoryService<ExpenseHistory> ExpenseHistoryService { get; set; }
		[Inject] IService<ExpenseCategories> ExpenseCategoryService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			expenseCategoryList = await ExpenseCategoryService.GetList();
			expenseTempHistoryList.Add(GetInitialTempExpenseHistory());
		}

		// Table functionality methods

		private void AddRow()
		{
			if (expenseTempHistoryList.Count >= 25)
			{
				Snackbar.Add("Max of 25 new records", Severity.Info);
				return;
			}

			expenseTempHistoryList.Add(GetNextRow());
		}

		private void DeleteRow()
		{
			var lastIndex = expenseTempHistoryList.LastOrDefault();
			expenseTempHistoryList.Remove(lastIndex);
		}

		private void RemoveElement(int id)
		{
			var element = expenseTempHistoryList.Where(e => e.ExpenseHistoryId == id).First();
			expenseTempHistoryList.Remove(element);
			ReIndexList();
		}

		private TempExpenseHistory GetInitialTempExpenseHistory()
		{
			return new() { ExpenseHistoryId = 1, ExpenseDate = DateTime.Now, ExpenseCategoryId = 1 };
		}

		private TempExpenseHistory GetNextRow()
		{
			return new() { ExpenseHistoryId = NextId(), ExpenseDate = DateTime.Now, ExpenseCategoryId = 1 };
		}

		private int NextId()
		{
			return expenseTempHistoryList.Count + 1;
		}

		private void ReIndexList()
		{
			int currentIndex = 1;

			foreach (var expense in expenseTempHistoryList)
			{
				expense.ExpenseHistoryId = currentIndex;
				currentIndex++;
			}
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

			foreach (var expense in expenseTempHistoryList)
			{
				ExpenseHistory newExpenseHistory = GetNewModel(expense);
				await SaveRecord(newExpenseHistory);
			}

			ClearListAndSuccessMessage();
		}

		private void ClearListAndSuccessMessage()
		{
			expenseTempHistoryList.Clear();
			expenseTempHistoryList.Add(GetInitialTempExpenseHistory());
			Snackbar.Add("New Expense Histories Added", Severity.Success);
		}

		private int CheckListForIssues()
		{
			foreach (var expense in expenseTempHistoryList)
			{
				if (string.IsNullOrWhiteSpace(expense.ExpenseName))
				{
					return expense.ExpenseHistoryId;
				}

				DateTime dateTime = (DateTime)expense.ExpenseDate;
				if (dateTime.Year < 2000)
				{
					return expense.ExpenseHistoryId;
				}
			}
			return 0;
		}

		private async Task SaveRecord(ExpenseHistory newExpenseHistory)
		{
			try
			{
				await ExpenseHistoryService.CreateRecord(newExpenseHistory);
			}
			catch (Exception e)
			{
				Snackbar.Add(e.Message, Severity.Error);
			}
		}

		// Convert TempExpenseHistory to ExpenseHistory to save to db
		private static ExpenseHistory GetNewModel(TempExpenseHistory expense)
		{
			return new()
			{
				ExpenseName = expense.ExpenseName,
				AmountPaid = expense.AmountPaid,
				ExpenseDate = (DateTime)expense.ExpenseDate,
				ExpenseCategoryId = expense.ExpenseCategoryId
			};
		}

		// Temp class added due to MudBlazor DateTimePicker needing DateTime? instead of DateTime to bind Date
		private class TempExpenseHistory
		{
			public int ExpenseHistoryId { get; set; }
			public string ExpenseName { get; set; }
			public decimal AmountPaid { get; set; }
			public DateTime? ExpenseDate { get; set; }
			public int ExpenseCategoryId { get; set; }
		}
	}
}