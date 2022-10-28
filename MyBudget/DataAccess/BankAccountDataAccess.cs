using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
    public class BankAccountDataAccess : IDataAccess<BankAccounts>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public BankAccountDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<BankAccounts> GetRecordByIdAsync(int id)
        {
            Initialize();
            return await _asyncConnection.Table<BankAccounts>()
                .Where(b => b.BankAccountId == id)
                .FirstAsync();
        }

        public async Task<List<BankAccounts>> GetListAsync()
        {
            Initialize();
            return await _asyncConnection.Table<BankAccounts>().ToListAsync();
        }

        public async Task<BankAccounts> CreateRecord(BankAccounts bankAccount)
        {
            try
            {
                await _asyncConnection.InsertAsync(bankAccount).ContinueWith((b) =>
                {
                    MyBudgetLogger.CreatedLogMessage(bankAccount);
                });
                return bankAccount;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(bankAccount, e);
                return new BankAccounts() { BankAccountId = 0 };
            }
        }

        public async Task<BankAccounts> UpdateRecordAsync(BankAccounts bankAccount)
        {
            try
            {
                await _asyncConnection.UpdateAsync(bankAccount).ContinueWith((b) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(bankAccount);
                });
                return bankAccount;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(bankAccount, e);
                return new BankAccounts() { BankAccountId = 0 };
            }
        }

        public async Task<BankAccounts> DeleteRecordAsync(BankAccounts bankAccount)
        {
            try
            {
                await _asyncConnection.DeleteAsync(bankAccount).ContinueWith((b) =>
                {
                    MyBudgetLogger.DeletedLogMessage(bankAccount);
                });
                return bankAccount;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(bankAccount, e);
                return new BankAccounts() { BankAccountId = 0 };
            }
        }

        public bool DoesNameExist(string bankAccountName)
        {
            int result;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                result = _connection.Table<BankAccounts>()
                    .Where(b => b.BankAccountName.ToLower() == bankAccountName.ToLower())
                    .Count();
            }

            return result > 0;
        }

        public string GetNameById(int id)
        {
            string bankAccountName;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                bankAccountName = _connection.Table<BankAccounts>()
                    .Where(b => b.BankAccountId == id)
                    .Select(b => b.BankAccountName)
                    .SingleOrDefault();
            }

            return bankAccountName;
        }

        // private methods

        private void Initialize()
        {
            if (_asyncConnection != null) { return; }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
        }
    }
}
