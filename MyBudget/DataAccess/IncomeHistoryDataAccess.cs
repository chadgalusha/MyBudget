using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
	public class IncomeHistoryDataAccess : IHistoryDataAccess<IncomeHistory>
	{
		private readonly string _dbPath;
		private SQLiteAsyncConnection _asyncConnection;
		private SQLiteConnection _connection;

		public IncomeHistoryDataAccess()
		{
			_dbPath = DatabaseHelper.GetDbPath();
		}

		public async Task<IncomeHistory> GetRecordByIdAsync(int id)
		{
			AsyncInitialize();
			return await _asyncConnection.Table<IncomeHistory>()
				.Where(i => i.IncomeHistoryId == id)
				.FirstAsync();
		}

		public async Task<List<IncomeHistory>> GetListAsync()
		{
			AsyncInitialize();
			return await _asyncConnection.Table<IncomeHistory>().ToListAsync();
		}

		public async Task<IncomeHistory> CreateRecordAsync(IncomeHistory newIncomeHistory)
		{
			AsyncInitialize();

			try
			{
				await _asyncConnection.InsertAsync(newIncomeHistory).ContinueWith((i) =>
				{
					MyBudgetLogger.CreatedLogMessage(newIncomeHistory);
				});
				return newIncomeHistory;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newIncomeHistory, e);
				return new IncomeHistory() { IncomeHistoryId = 0 };
			}
		}

		public async Task<IncomeHistory> UpdateRecordAsync(IncomeHistory incomehistory)
		{
			try
			{
				await _asyncConnection.UpdateAsync(incomehistory).ContinueWith((i) =>
				{
					MyBudgetLogger.UpdatedLogMessage(incomehistory);
				});

				return incomehistory;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(incomehistory, e);
				return new IncomeHistory() { IncomeHistoryId = 0 };
			}
		}

		public async Task<IncomeHistory> DeleteRecordAsync(IncomeHistory incomehistory)
		{
			try
			{
				await _asyncConnection.DeleteAsync(incomehistory).ContinueWith((i) =>
				{
					MyBudgetLogger.DeletedLogMessage(incomehistory);
				});

				return incomehistory;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(incomehistory, e);
				return new IncomeHistory() { IncomeHistoryId = 0 };
			}
		}

        // Due to issue with SQLite interface, result must go to array before where clauses.
        public decimal[] GetHistoryArrayForMonth(int year, int month)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				var result = _connection.Table<IncomeHistory>()
					.ToArray()
					.Where(i => i.IncomeDate.Year == year)
					.Where(i => i.IncomeDate.Month == month)
					.Select(i => i.IncomeAmount);

				return result.ToArray();
			}
		}

		public decimal[] GetHistoryArrayForYear(int year)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				var result = _connection.Table<IncomeHistory>()
					.ToArray()
					.Where(i => i.IncomeDate.Year == year)
					.Select(i => i.IncomeAmount);

				return result.ToArray();
			}
		}

		public List<IncomeHistory> GetListByYearMonth(int year, int month)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				var result = _connection.Table<IncomeHistory>()
					.ToArray()
					.Where(i => i.IncomeDate.Year == year)
					.Where(i => i.IncomeDate.Month == month);

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
