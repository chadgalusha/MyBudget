using MyBudget.DataAccess;
using MyBudget.Helpers;
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
            if (IsBankAccountTypeNameAlreadyUsed(newType.BankAccountType) == true)
            {
                return new BankAccountTypes() { BankAccountTypeId = -1 };
            }

            try
            {
                BankAccountTypes newBankAccountType = new()
                {
                    BankAccountType = newType.BankAccountType
                };

                return await _bankAccountTypeDataAccess.CreateRecord(newBankAccountType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newType, e);
                return new BankAccountTypes() { BankAccountTypeId = 0 };
            }
        }

        public async Task<BankAccountTypes> UpdateRecord(BankAccountTypes bankAccountType)
        {
            if (IsUpdatedBankAccountTypeNameModified(bankAccountType) == true)
            {
                if (IsBankAccountTypeNameAlreadyUsed(bankAccountType.BankAccountType) == true)
                {
                    return new BankAccountTypes() { BankAccountTypeId = -1 };
                }
            }

            try
            {
                return await _bankAccountTypeDataAccess.UpdateRecordAsync(bankAccountType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(bankAccountType, e);
                return new BankAccountTypes() { BankAccountTypeId = 0 };
            }
        }

        public async Task<BankAccountTypes> DeleteRecord(BankAccountTypes bankAccountType)
        {
            if (IsBankAccountTypeUsedByBankAccount(bankAccountType.BankAccountTypeId) == true)
            {
                return new BankAccountTypes() { BankAccountTypeId = -1 };
            }

            try
            {
                return await _bankAccountTypeDataAccess.DeleteRecordAsync(bankAccountType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(bankAccountType, e);
                return new BankAccountTypes() { BankAccountTypeId = 0 };
            }
        }

        // PRIVATE METHODS

        private bool IsBankAccountTypeNameAlreadyUsed(string bankAccountTypeName)
        {
            return _bankAccountTypeDataAccess.DoesTypeNameExist(bankAccountTypeName);
        }

        private bool IsUpdatedBankAccountTypeNameModified(BankAccountTypes bankAccountType)
        {
            string currentBankAccountTypeName = _bankAccountTypeDataAccess.GetNameOfTypeByID(bankAccountType.BankAccountTypeId);
            return !currentBankAccountTypeName.Equals(bankAccountType.BankAccountType);
        }

        private bool IsBankAccountTypeUsedByBankAccount(int bankAccountTypeId)
        {
            return _bankAccountTypeDataAccess.IsTypeUsedAndCannotBeDeleted(bankAccountTypeId);
        }
    }
}
