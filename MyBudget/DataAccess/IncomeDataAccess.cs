using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
	public class IncomeDataAccess : IDataAccess<Incomes>
	{
		private readonly string _dbPath;
		private SQLiteAsyncConnection _asyncConnection;
		private SQLiteConnection _connection;

		public IncomeDataAccess()
		{
			_dbPath = DatabaseHelper.GetDbPath();
		}

		public async Task<Incomes> GetRecordByIdAsync(int id)
		{
			Initialize();
			return await _asyncConnection.Table<Incomes>()
				.Where(i => i.IncomeId == id)
				.FirstAsync();
		}

		public async Task<List<Incomes>> GetListAsync()
		{
			Initialize();
			return await _asyncConnection.Table<Incomes>().ToListAsync();
		}

		public async Task<Incomes> CreateRecord(Incomes newIncome)
		{
			try
			{
				await _asyncConnection.InsertAsync(newIncome).ContinueWith((i) =>
				{
					MyBudgetLogger.CreatedLogMessage(newIncome);
				});
				return newIncome;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newIncome, e);
				return null;
			}
		}

		public async Task<Incomes> UpdateRecordAsync(Incomes income)
		{
			try
			{
				await _asyncConnection.UpdateAsync(income).ContinueWith((i) =>
				{
					MyBudgetLogger.UpdatedLogMessage(income);
				});
				return income;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(income, e);
				return null;
			}
		}

		public async Task<Incomes> DeleteRecordAsync(Incomes income)
		{
			try
			{
				await _asyncConnection.DeleteAsync(income).ContinueWith((i) =>
				{
					MyBudgetLogger.DeletedLogMessage(income);
				});
				return income;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(income, e);
				return null;
			}
		}

		public bool DoesNameExist(string incomeName)
		{
			int result;
			using (_connection = new SQLiteConnection(_dbPath))
			{
				result = _connection.Table<Incomes>()
					.Where(i => i.IncomeName.ToLower() == incomeName.ToLower())
					.Count();
			}

			return result > 0;
		}

		public string GetNameById(int id)
		{
			string incomeName;
			using (_connection = new SQLiteConnection(_dbPath))
			{
				incomeName = _connection.Table<Incomes>()
					.Where(i => i.IncomeId == id)
					.Select(i => i.IncomeName)
					.SingleOrDefault();
			}

			return incomeName;
		}

		// private methods

		private void Initialize()
		{
			if (_asyncConnection != null) { return; }

			_asyncConnection = new SQLiteAsyncConnection(_dbPath);
		}
	}
}
