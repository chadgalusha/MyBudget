using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
	public class ExpenseCategoriesService : IService<ExpenseCategories>
	{
		private readonly IDataAccess<ExpenseCategories> _expenseCategoriesDataAccess;
		private readonly IHistoryService<ExpenseHistory> _expenseHistoryService;

		public ExpenseCategoriesService(IDataAccess<ExpenseCategories> expenseCategoriesDataAccess,
			IHistoryService<ExpenseHistory> expenseHistoryService)
		{
			_expenseCategoriesDataAccess = expenseCategoriesDataAccess;
			_expenseHistoryService = expenseHistoryService;
		}

		public async Task<ExpenseCategories> GetById(int id)
		{
			return await _expenseCategoriesDataAccess.GetRecordByIdAsync(id);
		}

		public async Task<List<ExpenseCategories>> GetList()
		{
			return await _expenseCategoriesDataAccess.GetListAsync();
		}

		public async Task<ExpenseCategories> CreateRecord(ExpenseCategories expenseCategory)
		{
			if (IsExpenseCategoryNameAlreadyUsed(expenseCategory.ExpenseCategoryName) == true)
			{
				return new ExpenseCategories() { ExpenseCategoryId = -1 };
			}

			try
			{
				ExpenseCategories newExpenseCategory = new()
				{
					ExpenseCategoryName = expenseCategory.ExpenseCategoryName,
					ExpenseCategoryDescription = expenseCategory.ExpenseCategoryDescription
				};

				return await _expenseCategoriesDataAccess.CreateRecord(expenseCategory);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(expenseCategory, e);
				return new ExpenseCategories() { ExpenseCategoryId = 0 };
			}
		}

		public async Task<ExpenseCategories> UpdateRecord(ExpenseCategories expenseCategory)
		{
			if (IsUpdatedExpenseCategoryNameModified(expenseCategory) == true)
			{
				if (IsExpenseCategoryNameAlreadyUsed(expenseCategory.ExpenseCategoryName) == true)
				{
					return new ExpenseCategories() { ExpenseCategoryId = -1 };
				}
			}

			try
			{
				return await _expenseCategoriesDataAccess.UpdateRecordAsync(expenseCategory);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(expenseCategory, e);
				return new ExpenseCategories() { ExpenseCategoryId = 0 };
			}
		}

		public async Task<ExpenseCategories> DeleteRecord(ExpenseCategories expenseCategory)
		{
			bool isCategoryBeingUsed = await IsExpenseCategoryUsedInExpenseHistory(expenseCategory.ExpenseCategoryId);
			if (isCategoryBeingUsed == true)
			{
				return new ExpenseCategories() { ExpenseCategoryId = -1 };
			}

			try
			{
				return await _expenseCategoriesDataAccess.DeleteRecordAsync(expenseCategory);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(expenseCategory, e);
				return new ExpenseCategories() { ExpenseCategoryId = 0 };
			}
		}

		// PRIVATE METHODS

		private bool IsExpenseCategoryNameAlreadyUsed(string expenseCategoryname)
		{
			return _expenseCategoriesDataAccess.DoesNameExist(expenseCategoryname);
		}

		private bool IsUpdatedExpenseCategoryNameModified(ExpenseCategories expenseCategory)
		{
			string currentExpenseCategoryname = _expenseCategoriesDataAccess.GetNameById(expenseCategory.ExpenseCategoryId);
			return !currentExpenseCategoryname.Equals(expenseCategory.ExpenseCategoryName);
		}

		private async Task<List<ExpenseHistory>> GetListExpenseHistories()
		{
			return await _expenseHistoryService.GetList();
		}

		private async Task<bool> IsExpenseCategoryUsedInExpenseHistory(int expenseCategoryId)
		{
			List<ExpenseHistory> expenseHistoryList = await GetListExpenseHistories();

			int result = expenseHistoryList.Where(e => e.ExpenseCategoryId == e.ExpenseCategoryId).Count();

			return result > 0;
		}
	}
}
