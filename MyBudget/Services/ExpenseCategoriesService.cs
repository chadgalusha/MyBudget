using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class ExpenseCategoriesService : IService<ExpenseCategories>
    {
        private readonly IDataAccess<ExpenseCategories> _expenseCategoriesDataAccess;

        public ExpenseCategoriesService(IDataAccess<ExpenseCategories> expenseCategoriesDataAccess)
        {
            _expenseCategoriesDataAccess = expenseCategoriesDataAccess;
        }

        public async Task<ExpenseCategories> GetById(int id)
        {
            return await _expenseCategoriesDataAccess.GetRecordByIdAsync(id);
        }

        public async Task<List<ExpenseCategories>> GetList()
        {
            return await _expenseCategoriesDataAccess.GetListAsync();
        }

        public async Task<ExpenseCategories> CreateRecord(ExpenseCategories newExpenseCategory)
        {
            return await _expenseCategoriesDataAccess.CreateRecord(newExpenseCategory);
        }

        public async Task<ExpenseCategories> UpdateRecord(ExpenseCategories expenseCategory)
        {
            return await _expenseCategoriesDataAccess.UpdateRecordAsync(expenseCategory);
        }

        public async Task<ExpenseCategories> DeleteRecord(ExpenseCategories expenseCategory)
        {
            return await _expenseCategoriesDataAccess.DeleteRecordAsync(expenseCategory);
        }
    }
}
