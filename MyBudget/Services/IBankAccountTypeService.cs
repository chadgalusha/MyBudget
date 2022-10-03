using MyBudget.Models;

namespace MyBudget.Services
{
    public interface IBankAccountTypeService
    {
        Task<BankAccountTypes> CreateRecord(BankAccountTypes newType);
        Task<BankAccountTypes> DeleteRecord(BankAccountTypes type);
        Task<BankAccountTypes> GetById(int id);
        Task<List<BankAccountTypes>> GetList();
        Task<BankAccountTypes> UpdateRecord(BankAccountTypes type);
    }
}