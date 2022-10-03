using MyBudget.Models;

namespace MyBudget.Services
{
    public interface IIncomeTypeService
    {
        Task<IncomeTypes> CreateRecord(IncomeTypes newType);
        Task<IncomeTypes> DeleteRecord(IncomeTypes type);
        Task<IncomeTypes> GetById(int id);
        Task<List<IncomeTypes>> GetList();
        Task<IncomeTypes> UpdateRecord(IncomeTypes incomeType);
    }
}