using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
	public class ExpenseTypeService : ITypeService<ExpenseTypes>
	{
		private readonly ITypeDataAccess<ExpenseTypes> _expenseTypeDataAccess;

		public ExpenseTypeService(ITypeDataAccess<ExpenseTypes> expenseTypeDataAccess)
		{
			_expenseTypeDataAccess = expenseTypeDataAccess;
		}

		public async Task<ExpenseTypes> GetById(int id)
		{
			return await _expenseTypeDataAccess.GetRecordByIdAsync(id);
		}

		public async Task<List<ExpenseTypes>> GetList()
		{
			return await _expenseTypeDataAccess.GetListAsync();
		}

		public async Task<ExpenseTypes> CreateRecord(ExpenseTypes newType)
		{
			if (IsExpenseTypeNameAlreadyUsed(newType.ExpenseType) == true)
			{
				return new ExpenseTypes() { ExpenseTypeId = -1 };
			}

			try
			{
				ExpenseTypes newExpenseType = new()
				{
					ExpenseType = newType.ExpenseType
				};

				return await _expenseTypeDataAccess.CreateRecord(newExpenseType);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newType, e);
				return new ExpenseTypes() { ExpenseTypeId = 0 };
			}
		}

		public async Task<ExpenseTypes> UpdateRecord(ExpenseTypes expenseType)
		{
			if (IsUpdatedExpenseTypeNameModified(expenseType) == true)
			{
				if (IsExpenseTypeNameAlreadyUsed(expenseType.ExpenseType) == true)
				{
					return new ExpenseTypes() { ExpenseTypeId = -1 };
				}
			}

			try
			{
				return await _expenseTypeDataAccess.UpdateRecordAsync(expenseType);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(expenseType, e);
				return new ExpenseTypes() { ExpenseTypeId = 0 };
			}
		}

		public async Task<ExpenseTypes> DeleteRecord(ExpenseTypes type)
		{
			if (IsExpenseTypeUsedByExpense(type.ExpenseTypeId) == true)
			{
				return new ExpenseTypes() { ExpenseTypeId = -1 };
			}

			try
			{
				return await _expenseTypeDataAccess.DeleteRecordAsync(type);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(type, e);
				return new ExpenseTypes() { ExpenseTypeId = 0 };
			}
		}

		// PRIVATE METHODS

		private bool IsExpenseTypeNameAlreadyUsed(string expenseTypeName)
		{
			return _expenseTypeDataAccess.DoesTypeNameExist(expenseTypeName);
		}

		// return false if NOT modified
		private bool IsUpdatedExpenseTypeNameModified(ExpenseTypes expenseType)
		{
			string currentExpenseTypeName = _expenseTypeDataAccess.GetNameOfTypeByID(expenseType.ExpenseTypeId);
			return !currentExpenseTypeName.Equals(expenseType.ExpenseType);
		}

		private bool IsExpenseTypeUsedByExpense(int expenseTypeId)
		{
			return _expenseTypeDataAccess.IsTypeUsedAndCannotBeDeleted(expenseTypeId);
		}
	}
}
