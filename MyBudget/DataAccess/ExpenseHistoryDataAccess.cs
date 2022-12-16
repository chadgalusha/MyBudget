using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
	public class ExpenseHistoryDataAccess : IHistoryDataAccess<ExpenseHistory>
	{
		private readonly string _dbPath;
		private SQLiteAsyncConnection _asyncConnection;
		private SQLiteConnection _connection;

		public ExpenseHistoryDataAccess()
		{
			_dbPath = DatabaseHelper.GetDbPath();
		}

		public Task<ExpenseHistory> GetRecordByIdAsync(int id)
		{
			AsyncInitialize();
			return _asyncConnection.Table<ExpenseHistory>()
				.Where(e => e.ExpenseHistoryId == id)
				.FirstAsync();
		}

		public Task<List<ExpenseHistory>> GetListAsync()
		{
			AsyncInitialize();
			return _asyncConnection.Table<ExpenseHistory>().ToListAsync();
		}

		public async Task<ExpenseHistory> CreateRecordAsync(ExpenseHistory newExpenseHistory)
		{
			AsyncInitialize();

			try
			{
				await _asyncConnection.InsertAsync(newExpenseHistory).ContinueWith((e) =>
				{
					MyBudgetLogger.CreatedLogMessage(newExpenseHistory);
				});

				return newExpenseHistory;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newExpenseHistory, e);
				return new ExpenseHistory() { ExpenseHistoryId = 0 };
			}
		}

		public async Task<ExpenseHistory> UpdateRecordAsync(ExpenseHistory expenseHistory)
		{
			try
			{
				await _asyncConnection.UpdateAsync(expenseHistory).ContinueWith((e) =>
				{
					MyBudgetLogger.UpdatedLogMessage(expenseHistory);
				});

				return expenseHistory;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(expenseHistory, e);
				return new ExpenseHistory() { ExpenseHistoryId = 0 };
			}
		}

		public async Task<ExpenseHistory> DeleteRecordAsync(ExpenseHistory expenseHistory)
		{
			try
			{
				await _asyncConnection.DeleteAsync(expenseHistory).ContinueWith((e) =>
				{
					MyBudgetLogger.DeletedLogMessage(expenseHistory);
				});

				return expenseHistory;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(expenseHistory, e);
				return new ExpenseHistory() { ExpenseHistoryId = 0 };
			}
		}

        // Due to issue with SQLite API, result must go to array before where clauses.
        public decimal[] GetHistoryArrayForMonth(int year, int month)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				var result = _connection.Table<ExpenseHistory>()
					.ToArray()
					.Where(e => e.ExpenseDate.Year == year)
					.Where(e => e.ExpenseDate.Month == month)
					.Select(e => e.AmountPaid);

				return result.ToArray();
			}
		}

		public decimal[] GetHistoryArrayForYear(int year)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				var result = _connection.Table<ExpenseHistory>()
					.ToArray()
					.Where(e => e.ExpenseDate.Year == year)
					.Select(e => e.AmountPaid);

				return result.ToArray();
			}
		}

		public List<ExpenseHistory> GetListByYearMonth(int year, int month)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
                var result = _connection.Table<ExpenseHistory>()
                    .ToArray()
                    .Where(i => i.ExpenseDate.Year == year)
                    .Where(i => i.ExpenseDate.Month == month);

                return result.ToList();
            }
		}

		// PRIVATE METHODS

		private void AsyncInitialize()
		{
			if (_asyncConnection != null) { return; }

			_asyncConnection = new SQLiteAsyncConnection(_dbPath);
		}
	}
}
