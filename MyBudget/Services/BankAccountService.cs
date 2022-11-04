using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
	public class BankAccountService : IService<BankAccounts>
	{
		private readonly IDataAccess<BankAccounts> _bankAccountsDataAccess;

		public BankAccountService(IDataAccess<BankAccounts> bankAccountsDataAccess)
		{
			_bankAccountsDataAccess = bankAccountsDataAccess;
		}

		public async Task<BankAccounts> GetById(int id)
		{
			return await _bankAccountsDataAccess.GetRecordByIdAsync(id);
		}

		public async Task<List<BankAccounts>> GetList()
		{
			return await _bankAccountsDataAccess.GetListAsync();
		}

		public async Task<BankAccounts> CreateRecord(BankAccounts newBankAccount)
		{
			if (IsBankAccountNameAlreadyUsed(newBankAccount.BankAccountName) == true)
			{
				return new BankAccounts() { BankAccountId = -1 };
			}

			try
			{
				BankAccounts bankAccount = new()
				{
					BankAccountName = newBankAccount.BankAccountName,
					BankAccountTypeId = newBankAccount.BankAccountTypeId,
					Balance = newBankAccount.Balance
				};

				return await _bankAccountsDataAccess.CreateRecord(bankAccount);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorCreating(newBankAccount, e);
				return new BankAccounts() { BankAccountId = 0 };
			}
		}

		public async Task<BankAccounts> UpdateRecord(BankAccounts bankAccount)
		{
			if (IsUpdatedBankAccountNameModified(bankAccount) == true)
			{
				if (IsBankAccountNameAlreadyUsed(bankAccount.BankAccountName) == true)
				{
					return new BankAccounts() { BankAccountId = -1 };
				}
			}

			try
			{
				return await _bankAccountsDataAccess.UpdateRecordAsync(bankAccount);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorUpdating(bankAccount, e);
				return new BankAccounts() { BankAccountId = 0 };
			}
		}

		public async Task<BankAccounts> DeleteRecord(BankAccounts bankAccount)
		{
			try
			{
				return await _bankAccountsDataAccess.DeleteRecordAsync(bankAccount);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorDeleting(bankAccount, e);
				return new BankAccounts() { BankAccountId = 0 };
			}
		}

		// PRIVATE METHODS

		private bool IsBankAccountNameAlreadyUsed(string bankAccountName)
		{
			return _bankAccountsDataAccess.DoesNameExist(bankAccountName);
		}

		// return false if NOT modified
		private bool IsUpdatedBankAccountNameModified(BankAccounts bankAccount)
		{
			string currentBankAccountName = _bankAccountsDataAccess.GetNameById(bankAccount.BankAccountId);
			return !currentBankAccountName.Equals(bankAccount.BankAccountName);
		}
	}
}
