using MyBudget.DataAccess;
using MyBudget.Models;
using Serilog;

namespace MyBudget.Services
{
	public class ExpenseService : IService<Expenses>
	{
		private readonly IDataAccess<Expenses> _expenseDataAccess;

		public ExpenseService(IDataAccess<Expenses> expenseDataAccess)
		{
			_expenseDataAccess = expenseDataAccess;
		}

		public async Task<Expenses> GetById(int id)
		{
			return await _expenseDataAccess.GetRecordByIdAsync(id);
		}

		public async Task<List<Expenses>> GetList()
		{
			return await _expenseDataAccess.GetListAsync();
		}

		public async Task<Expenses> CreateRecord(Expenses newExpense)
		{
			if (IsExpenseNameAlreadyUsed(newExpense.ExpensesName) == true)
			{
				return new Expenses() { ExpensesId = -1 };
			}

			try
			{
				Expenses expense = new()
				{
					ExpensesName = newExpense.ExpensesName,
					ExpenseTypeId = newExpense.ExpenseTypeId,
					PaymentFrequencyTypeId = newExpense.PaymentFrequencyTypeId,
					ExpenseAmount = newExpense.ExpenseAmount,
					InitialExpenseDate = newExpense.InitialExpenseDate,
				};

				return await _expenseDataAccess.CreateRecord(expense);
			}
			catch (Exception e)
			{
				Log.Error($"Error creating new expense: {e.Message}");
				return new Expenses() { ExpensesId = 0 };
			}
		}

		public async Task<Expenses> UpdateRecord(Expenses expense)
		{
			if (IsUpdatedExpenseNameModified(expense) == true)
			{
				if (IsExpenseNameAlreadyUsed(expense.ExpensesName) == true)
				{
					return new Expenses() { ExpensesId = -1 };
				}
			}

			try
			{
				return await _expenseDataAccess.UpdateRecordAsync(expense);
			}
			catch (Exception e)
			{
				Log.Error($"Error updating expense: {e.Message}");
				return new Expenses() { ExpensesId = 0 };
			}
		}

		public async Task<Expenses> DeleteRecord(Expenses expense)
		{
			try
			{
				return await _expenseDataAccess.DeleteRecordAsync(expense);
			}
			catch (Exception e)
			{
				Log.Error($"Error deleting expense: {e.Message}");
				return new Expenses() { ExpensesId = 0 };
			}
		}

        #region Private Methods

        private bool IsExpenseNameAlreadyUsed(string expenseName)
		{
			return _expenseDataAccess.DoesNameExist(expenseName);
		}

		private bool IsUpdatedExpenseNameModified(Expenses expense)
		{
			string currentExpenseName = _expenseDataAccess.GetNameById(expense.ExpensesId);
			return !currentExpenseName.Equals(expense.ExpensesName);
		}

        #endregion
    }
}
