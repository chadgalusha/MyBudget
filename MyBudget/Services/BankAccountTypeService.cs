using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class BankAccountTypeService : ITypeService<BankAccountTypes>
    {
        private readonly ITypeDataAccess<BankAccountTypes> _bankAccountTypeDataAccess;

        public BankAccountTypeService(ITypeDataAccess<BankAccountTypes> bankAccountTypeDataAccess)
        {
            _bankAccountTypeDataAccess = bankAccountTypeDataAccess;
        }

        public async Task<BankAccountTypes> GetById(int id)
        {
            return await _bankAccountTypeDataAccess.GetRecordByIdAsync(id);
        }

        public async Task<List<BankAccountTypes>> GetList()
        {
            return await _bankAccountTypeDataAccess.GetListAsync();
        }

        public async Task<BankAccountTypes> CreateRecord(BankAccountTypes newType)
        {
            return await _bankAccountTypeDataAccess.CreateRecord(newType);
        }

        public async Task<BankAccountTypes> UpdateRecord(BankAccountTypes type)
        {
            return await _bankAccountTypeDataAccess.UpdateRecordAsync(type);
        }

        public async Task<BankAccountTypes> DeleteRecord(BankAccountTypes type)
        {
            return await _bankAccountTypeDataAccess.DeleteRecordAsync(type);
        }
    }
}
