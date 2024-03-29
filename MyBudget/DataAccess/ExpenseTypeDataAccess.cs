﻿using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
	public class ExpenseTypeDataAccess : ITypeDataAccess<ExpenseTypes>
	{
		private readonly string _dbPath;
		private SQLiteAsyncConnection _asyncConnection;
		private SQLiteConnection _connection;

		public ExpenseTypeDataAccess()
		{
			_dbPath = DatabaseHelper.GetDbPath();
		}

		public async Task<ExpenseTypes> GetRecordByIdAsync(int id)
		{
			await InitializeAsync();
			return await _asyncConnection.Table<ExpenseTypes>()
				.Where(e => e.ExpenseTypeId == id)
				.FirstAsync();
		}

		public async Task<List<ExpenseTypes>> GetListAsync()
		{
			await InitializeAsync();
			return await _asyncConnection.Table<ExpenseTypes>().ToListAsync();
		}

		public async Task<ExpenseTypes> CreateRecord(ExpenseTypes newType)
		{
			try
			{
				await _asyncConnection.InsertAsync(newType);
				MyBudgetLogger.CreatedLogMessage(newType);
				return newType;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newType, e);
				return new ExpenseTypes() { ExpenseTypeId = 0 };
			}
		}

		public async Task<ExpenseTypes> UpdateRecordAsync(ExpenseTypes type)
		{
			try
			{
				await _asyncConnection.UpdateAsync(type);
				MyBudgetLogger.UpdatedLogMessage(type);
				return type;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(type, e);
				return new ExpenseTypes() { ExpenseTypeId = 0 };
			}
		}

		public async Task<ExpenseTypes> DeleteRecordAsync(ExpenseTypes type)
		{
			try
			{
				await _asyncConnection.DeleteAsync(type);
				MyBudgetLogger.DeletedLogMessage(type);
				return type;
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(type, e);
				return new ExpenseTypes() { ExpenseTypeId = 0 };
			}
		}

		public bool DoesTypeNameExist(string expenseTypeName)
		{
			int result;
			using (_connection = new SQLiteConnection(_dbPath))
			{
				result = _connection.Table<ExpenseTypes>()
					.Where(e => e.ExpenseType.ToLower() == expenseTypeName.ToLower())
					.Count();
			}

			return result > 0;
		}

		public string GetNameOfTypeByID(int id)
		{
			string expenseTypeName;
			using (_connection = new SQLiteConnection(_dbPath))
			{
				expenseTypeName = _connection.Table<ExpenseTypes>()
					.Where(e => e.ExpenseTypeId == id)
					.Select(e => e.ExpenseType)
					.SingleOrDefault();
			}

			return expenseTypeName;
		}

		public bool IsTypeUsedAndCannotBeDeleted(int expenseTypeId)
		{
			int result;
			using (_connection = new SQLiteConnection(_dbPath))
			{
				result = _connection.Table<Expenses>()
					.Where(e => e.ExpenseTypeId == expenseTypeId)
					.Count();
			}

			return result > 0;
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
			var initialValuesArray = ExpenseTypes.InitialValues();

			foreach (var value in initialValuesArray)
			{
				ExpenseTypes newType = new()
				{
					ExpenseType = value
				};

				await CreateRecord(newType);
			}
		}
	}
}
