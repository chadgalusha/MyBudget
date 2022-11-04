using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
	public class IncomeTypeDataAccess : ITypeDataAccess<IncomeTypes>
	{
		private readonly string _dbPath;
		private SQLiteAsyncConnection _asyncConnection;
		private SQLiteConnection _connection;

		public IncomeTypeDataAccess()
		{
			_dbPath = DatabaseHelper.GetDbPath();
		}

		public async Task<IncomeTypes> GetRecordByIdAsync(int id)
		{
			await InitializeAsync();
			return await _asyncConnection.Table<IncomeTypes>()
				.Where(i => i.IncomeTypeId == id)
				.FirstAsync();
		}

		public async Task<List<IncomeTypes>> GetListAsync()
		{
			await InitializeAsync();
			return await _asyncConnection.Table<IncomeTypes>().ToListAsync();
		}

		public async Task<IncomeTypes> CreateRecord(IncomeTypes newType)
		{
			try
			{
				await _asyncConnection.InsertAsync(newType).ContinueWith((i) =>
				{
					MyBudgetLogger.CreatedLogMessage(newType);
				});
				return newType;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newType, e);
				return new IncomeTypes() { IncomeTypeId = 0 };
			}
		}

		public async Task<IncomeTypes> UpdateRecordAsync(IncomeTypes type)
		{
			try
			{
				await _asyncConnection.UpdateAsync(type).ContinueWith((i) =>
				{
					MyBudgetLogger.UpdatedLogMessage(type);
				});
				return type;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(type, e);
				return new IncomeTypes() { IncomeTypeId = 0 };
			}
		}

		public async Task<IncomeTypes> DeleteRecordAsync(IncomeTypes type)
		{
			try
			{
				await _asyncConnection.DeleteAsync(type).ContinueWith((i) =>
				{
					MyBudgetLogger.DeletedLogMessage(type);
				});
				return type;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(type, e);
				return new IncomeTypes() { IncomeTypeId = 0 };
			}
		}

		public bool DoesTypeNameExist(string incomeTypeName)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				int result = _connection.Table<IncomeTypes>()
				.Where(i => i.IncomeType.ToLower() == incomeTypeName.ToLower())
				.Count();

				return result > 0;
			}
		}

		public string GetNameOfTypeByID(int id)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				string incomeTypeName = _connection.Table<IncomeTypes>()
				.Where(i => i.IncomeTypeId == id)
				.Select(i => i.IncomeType)
				.SingleOrDefault();

				return incomeTypeName;
			}
		}

		public bool IsTypeUsedAndCannotBeDeleted(int incomeTypeId)
		{
			using (_connection = new SQLiteConnection(_dbPath))
			{
				int result = _connection.Table<Incomes>()
				.Where(i => i.IncomeTypeId == incomeTypeId)
				.Count();

				return result > 0;
			}
		}

		// private methods

		private async Task InitializeAsync()
		{
			if (_asyncConnection != null) { return; }

			_asyncConnection = new SQLiteAsyncConnection(_dbPath);

			if (await DoesTableHaveValuesAsync() == false)
			{
				await InitializeTableValuesAsync();
			}
		}

		private async Task<bool> DoesTableHaveValuesAsync()
		{
			var listOfValues = await GetListAsync();
			return listOfValues.Any();
		}

		private async Task InitializeTableValuesAsync()
		{
			var initialValuesArray = IncomeTypes.InitialValues();

			foreach (var value in initialValuesArray)
			{
				IncomeTypes newType = new()
				{
					IncomeType = value
				};

				await CreateRecord(newType);
			}
		}
	}
}
