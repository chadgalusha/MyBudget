using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyBudget.Components;
using MyBudget.Helpers;
using MyBudget.Models;
using MyBudget.Services;

namespace MyBudget.Pages
{
    public partial class CalendarPage
    {
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] IHistoryService<ExpenseHistory> ExpenseHistoryService { get; set; }
        [Inject] IService<ExpenseCategories> ExpenseCategoryService { get; set; }
		[Inject] IHistoryService<IncomeHistory> IncomeHistoryService { get; set; }
		[Inject] ICalendarProcessor CalendarProcessor { get; set; }

		private DateTime currentDateTime;
		private int year;
        private int month;
        private int today;
        private int startingIndex;
        private int endingIndex;

        // selectedDateTime for calendar menu
        private DateTime selectedDateTime;

        // Component variable
        private ExpenseHistoryForm expenseHistoryForm;
		private IncomeHistoryForm incomeHistoryForm;

        // Model variables
        private ExpenseHistory expenseHistory = new();
        private List<ExpenseCategories> expenseCategoryList = new();
        private IncomeHistory incomeHistory = new();
        private List<IncomeHistory> thisMonthIncomeHistoryList;
        private List<ExpenseHistory> thisMonthExpenseHistoryList;

        protected override async Task OnInitializedAsync()
        {
            currentDateTime = DateTime.Now;
            Set_Year_Month_Today(currentDateTime);
            SetIndexes();
            selectedDateTime = currentDateTime;
            expenseHistory = GetNewExpenseHistory();
            expenseCategoryList = await ExpenseCategoryService.GetList();
            incomeHistory = GetNewIncomeHistory();
            GetHistoryLists();
        }

        // Calendar methods
        void Set_Year_Month_Today(DateTime dateTime)
        {
            year = dateTime.Year;
            month = dateTime.Month;
            today = dateTime.Day;
        }

        void SetIndexes()
        {
            startingIndex = GetFirstOfMonthStartingIndex();
            endingIndex = startingIndex + GetNumberOfDaysInMonth() - 1;
        }

        void GetHistoryLists()
        {
            thisMonthIncomeHistoryList = GetIncomeHistoryList(year, month);
            thisMonthExpenseHistoryList = GetExpenseHistoryList(year, month);
        }

        int GetFirstOfMonthStartingIndex()
        {
            return CalendarProcessor.IntForFirstDayOfMonth(year, month);
        }

        int GetNumberOfDaysInMonth()
        {
            return CalendarProcessor.NumberDaysInMonth(year, month);
        }

        string GetMonthString(int month)
        {
            return CalendarProcessor.GetMonthString(month);
        }

        void UpdateCurrentDay(int updatedDate)
        {
            if (updatedDate == selectedDateTime.Day)
            {
                return;
            }

            if (updatedDate < selectedDateTime.Day)
            {
                selectedDateTime = selectedDateTime.AddDays(-(selectedDateTime.Day - updatedDate));
            }

            if (updatedDate > selectedDateTime.Day)
            {
                selectedDateTime = selectedDateTime.AddDays(updatedDate - selectedDateTime.Day);
            }
        }

        async void EditIncomeHistory(int id)
        {
            incomeHistory = await GetIncomeHistoryById(id);
            CancelExpenseHistory();
            ShowIncomeHistoryForm();
        }

        async void EditExpenseHistory(int id)
        {
            expenseHistory = await GetExpenseHistoryById(id);
            CancelIncomeHistory();
            ShowExpenseHistoryForm();
        }

        void BackOneMonth()
        {
            selectedDateTime = CalendarProcessor.PreviousMonth(selectedDateTime);
            Set_Year_Month_Today(selectedDateTime);
            SetIndexes();
            GetHistoryLists();
        }

        void ForwardOneMonth()
        {
            selectedDateTime = CalendarProcessor.NextMonth(selectedDateTime);
            Set_Year_Month_Today(selectedDateTime);
            SetIndexes();
            GetHistoryLists();
        }

        #region Expense History methods
        async Task<ExpenseHistory> GetExpenseHistoryById(int id)
        {
            return await ExpenseHistoryService.GetById(id);
        }

        List<ExpenseHistory> GetExpenseHistoryList(int year, int month)
        {
            return ExpenseHistoryService.GetListByYearMonth(year, month);
        }

        ExpenseHistory GetNewExpenseHistory()
        {
            return new()
            {ExpenseHistoryId = 0, ExpenseDate = selectedDateTime, ExpenseCategoryId = 1};
        }

        async Task SaveExpenseHistoryAsync(ExpenseHistory expense)
        {
            try
            {
                await ExpenseHistoryService.CreateRecord(expense);
                thisMonthExpenseHistoryList.Add(expense);
                Snackbar.Add($"Created new Expense History: {expense.ExpenseName} {expense.ExpenseDate}", Severity.Success);
                CancelExpenseHistory();
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        async Task UpdateExpenseHistoryAsync(ExpenseHistory expense)
        {
            try
            {
                await ExpenseHistoryService.UpdateRecord(expense);
                Snackbar.Add($"Expense Updated: {expense.ExpenseName} - {expense.ExpenseDate}", Severity.Success);
                CancelExpenseHistory();
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        async Task DeleteConfirmExpenseHistory(ExpenseHistory expenseHistory)
        {
            bool? confirmed = await DialogService.ShowMessageBox("Warning", $"Permanently Delete {expenseHistory.ExpenseName} : {expenseHistory.ExpenseDate.ToShortDateString()}?", yesText: "Delete", cancelText: "Cancel");
            if (confirmed == true)
            {
                await DeleteExpenseHistoryAsync(expenseHistory);
            }
        }

        async Task DeleteExpenseHistoryAsync(ExpenseHistory expense)
        {
            try
            {
                await ExpenseHistoryService.DeleteRecord(expense);
                thisMonthExpenseHistoryList = GetExpenseHistoryList(year, month); // a hack until I can update / re-render to remove deleted item
                Snackbar.Add($"Successfully deleted {expense.ExpenseName} : {expense.ExpenseDate.ToShortDateString()}", Severity.Success);
                CancelExpenseHistory();
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        #endregion

        #region Income History methods
        async Task<IncomeHistory> GetIncomeHistoryById(int id)
        {
            return await IncomeHistoryService.GetById(id);
        }

        List<IncomeHistory> GetIncomeHistoryList(int year, int month)
        {
            return IncomeHistoryService.GetListByYearMonth(year, month);
        }

        IncomeHistory GetNewIncomeHistory()
        {
            return new()
            {IncomeHistoryId = 0, IncomeDate = selectedDateTime};
        }

        async Task SaveIncomeHistoryAsync(IncomeHistory income)
        {
            try
            {
                await IncomeHistoryService.CreateRecord(income);
                thisMonthIncomeHistoryList.Add(income);
                Snackbar.Add($"Created new Income history: {income.IncomeName} {income.IncomeDate}", Severity.Success);
                CancelIncomeHistory();
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        async Task UpdateIncomeHistoryAsync(IncomeHistory income)
        {
            try
            {
                await IncomeHistoryService.UpdateRecord(income);
				Snackbar.Add($"Income Updated: {income.IncomeName} - {income.IncomeDate}", Severity.Success);
                CancelIncomeHistory();
			}
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        async Task DeleteConfirmIncomeHistory(IncomeHistory incomeHistory)
        {
            bool? confirmed = await DialogService.ShowMessageBox("Warning", $"Permanently Delete {incomeHistory.IncomeName} : {incomeHistory.IncomeDate.ToShortDateString()}?", yesText: "Delete", cancelText: "Cancel");
            if (confirmed == true)
            {
                await DeleteIncomeHistoryAsync(incomeHistory);
            }
        }

        async Task DeleteIncomeHistoryAsync(IncomeHistory income)
        {
            try
            {
                await IncomeHistoryService.DeleteRecord(income);
                thisMonthIncomeHistoryList = GetIncomeHistoryList(year, month);
                Snackbar.Add($"Successfully deleted {income.IncomeName} : {income.IncomeDate.ToShortDateString()}", Severity.Success);
                CancelIncomeHistory();
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
        }

        #endregion

        // Button methods
        void CancelExpenseHistory()
        {
            HideExpenseHistoryForm();
            expenseHistory = GetNewExpenseHistory();
        }

        void TurnOnExpenseHistory()
        {
            expenseHistory = GetNewExpenseHistory();
            HideIncomeHistoryForm();
            ShowExpenseHistoryForm();
        }

        void CancelIncomeHistory()
        {
            HideIncomeHistoryForm();
            incomeHistory = GetNewIncomeHistory();
        }

        void TurnOnIncomeHistory()
        {
            incomeHistory = GetNewIncomeHistory();
            HideExpenseHistoryForm();
            ShowIncomeHistoryForm();
        }

        // Component visibility toggles
        private void ShowExpenseHistoryForm() => expenseHistoryForm.SetVisible(true);
        private void HideExpenseHistoryForm() => expenseHistoryForm.SetVisible(false);
        private void ShowIncomeHistoryForm() => incomeHistoryForm.SetVisible(true);
        private void HideIncomeHistoryForm() => incomeHistoryForm.SetVisible(false);
    }
}