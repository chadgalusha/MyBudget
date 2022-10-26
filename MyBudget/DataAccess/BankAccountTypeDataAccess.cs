using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;
using System;

namespace MyBudget.DataAccess
{
    public class BankAccountTypeDataAccess : ITypeDataAccess<BankAccountTypes>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public BankAccountTypeDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<BankAccountTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<BankAccountTypes>()
                .Where(b => b.BankAccountTypeId == id)
                .FirstAsync();
        }

        public async Task<List<BankAccountTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<BankAccountTypes>().ToListAsync();
        }

        public async Task<BankAccountTypes> CreateRecord(BankAccountTypes newType)
        {
            try
            {
                await _asyncConnection.InsertAsync(newType).ContinueWith((b) =>
                {
                    MyBudgetLogger.CreatedLogMessage(newType);
                });
                return newType;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newType, e);
                return new BankAccountTypes() { BankAccountTypeId = 0 };
            }
        }

        public async Task<BankAccountTypes> UpdateRecordAsync(BankAccountTypes type)
        {
            try
            {
                await _asyncConnection.UpdateAsync(type).ContinueWith((b) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(type);
                });
                return type;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(type, e);
                return new BankAccountTypes() { BankAccountTypeId = 0 };
            }
        }

        public async Task<BankAccountTypes> DeleteRecordAsync(BankAccountTypes type)
        {
            try
            {
                await _asyncConnection.DeleteAsync(type).ContinueWith((b) =>
                {
                    MyBudgetLogger.DeletedLogMessage(type);
                });
                return type;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(type, e);
                return new BankAccountTypes() { BankAccountTypeId = 0 };
            }
        }

        public bool DoesTypeNameExist(string typeName)
        {
            using (_connection = new SQLiteConnection(_dbPath))
            {
                int result = _connection.Table<BankAccountTypes>()
                    .Where(b => b.BankAccountType.ToLower() == typeName.ToLower())
                    .Count();

                return result > 0;
            }
        }

        public string GetNameOfTypeByID(int typeId)
        {
            using (_connection = new SQLiteConnection(_dbPath))
            {
                string bankAccountTypeName = _connection.Table<BankAccountTypes>()
                    .Where(b => b.BankAccountTypeId == typeId)
                    .Select(b => b.BankAccountType)
                    .SingleOrDefault();

                return bankAccountTypeName;
            }
        }

        public bool IsTypeUsedAndCannotBeDeleted(int typeId)
        {
            using (_connection = new SQLiteConnection(_dbPath))
            {
                int result = _connection.Table<BankAccounts>()
                    .Where(b => b.BankAccountTypeId == typeId)
                    .Count();

                return result > 0;
            }
        }

        // private methods

        private async Task InitializeAsync()
        {
            if (_asyncConnection != null)
            {
                return;
            }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
            //await _connection.CreateTableAsync<BankAccountTypes>();

            if (await DoesTableHaveValuesAsync() == false)
            {
                await InitializeTableValuesAsync();
            }
        }

        private async Task<bool> DoesTableHaveValuesAsync()
        {
            var listOfValues = await GetListAsync();
            return listOfValues.Any();
        }

        private async Task InitializeTableValuesAsync()
        {
            var initialValuesArray = BankAccountTypes.InitialValues();

            foreach (var value in initialValuesArray)
            {
                BankAccountTypes newType = new()
                {
                    BankAccountType = value
                };

                await CreateRecord(newType);
            }
        }
    }
}
