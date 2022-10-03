using MyBudget.Models;

namespace MyBudget.Services
{
    public interface IExpenseTypeService
    {
        Task<ExpenseTypes> CreateRecord(ExpenseTypes newType);
        Task<ExpenseTypes> DeleteRecord(ExpenseTypes type);
        Task<ExpenseTypes> GetById(int id);
        Task<List<ExpenseTypes>> GetList();
        Task<ExpenseTypes> UpdateRecord(ExpenseTypes type);
    }
}