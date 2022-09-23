using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class ExpenseTypeService
    {
        private readonly ExpenseTypeDataAccess _expenseTypeDataAccess;

        public ExpenseTypeService(ExpenseTypeDataAccess incomeTypeDataAccess)
        {
            _expenseTypeDataAccess = incomeTypeDataAccess;
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
            return await _expenseTypeDataAccess.CreateRecord(newType);
        }

        public async Task<ExpenseTypes> UpdateRecord(ExpenseTypes type)
        {
            return await _expenseTypeDataAccess.UpdateRecordAsync(type);
        }

        public async Task<ExpenseTypes> DeleteRecord(ExpenseTypes type)
        {
            return await _expenseTypeDataAccess.DeleteRecordAsync(type);
        }
    }
}
