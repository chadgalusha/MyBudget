using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.DataAccess
{
	public class BankAccountDataAccess : IDataAccess<BankAccounts>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public BankAccountDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<BankAccounts> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<BankAccounts>()
                .Where(b => b.BankAccountId == id)
                .FirstAsync();
        }

        public async Task<List<BankAccounts>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<BankAccounts>().ToListAsync();
        }

        public async Task<BankAccounts> CreateRecord(BankAccounts bankAccount)
        {
            try
            {
                await _asyncConnection.InsertAsync(bankAccount).ContinueWith((b) =>
                {
                    Log.Information($"Bank account created: {bankAccount.BankAccountName}");
                });
                return bankAccount;
            }
            catch (Exception e)
            {
                Log.Error($"Error creating new bank account: {e.Message}");
                return new BankAccounts() { BankAccountId = 0 };
            }
        }

        public async Task<BankAccounts> UpdateRecordAsync(BankAccounts bankAccount)
        {
            try
            {
                await _asyncConnection.UpdateAsync(bankAccount).ContinueWith((b) =>
                {
                    Log.Information($"Bank account updated: {bankAccount.BankAccountId}, {bankAccount.BankAccountName}");
                });
                return bankAccount;
            }
            catch (Exception e)
            {
                Log.Error($"Error updating bank account: {e.Message}");
                return new BankAccounts() { BankAccountId = 0 };
            }
        }

        public async Task<BankAccounts> DeleteRecordAsync(BankAccounts bankAccount)
        {
            try
            {
                await _asyncConnection.DeleteAsync(bankAccount).ContinueWith((b) =>
                {
                    Log.Information($"Bank account deleted: {bankAccount.BankAccountId}, {bankAccount.BankAccountName}");
                });
                return bankAccount;
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting bank account: {e.Message}");
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

        private async Task InitializeAsync()
        {
            if (_asyncConnection != null)
            {
                return;
            }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
            await _asyncConnection.CreateTableAsync<BankAccounts>().ContinueWith((results) =>
            {
                Log.Information($"Bank Account table created: {results.Result}");
            });
        }
    }
}
