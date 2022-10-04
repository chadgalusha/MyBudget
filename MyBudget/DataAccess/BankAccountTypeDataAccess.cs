using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class BankAccountTypeDataAccess : ITypeDataAccess<BankAccountTypes>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public BankAccountTypeDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<BankAccountTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _connection.Table<BankAccountTypes>()
                .Where(b => b.BankAccountTypeId == id)
                .FirstAsync();
        }

        public async Task<List<BankAccountTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<BankAccountTypes>().ToListAsync();
        }

        public async Task<BankAccountTypes> CreateRecord(BankAccountTypes newType)
        {
            try
            {
                await _connection.InsertAsync(newType);
                return newType;
            }
            catch (Exception e)
            {
                Log.Error($"Error inserting new record: {e.Message}");
                return null;
            }
        }

        public async Task<BankAccountTypes> UpdateRecordAsync(BankAccountTypes type)
        {
            try
            {
                await _connection.UpdateAsync(type);
                return type;
            }
            catch (Exception e)
            {
                Log.Error($"Error updating record: {e.Message}");
                return null;
            }
        }

        public async Task<BankAccountTypes> DeleteRecordAsync(BankAccountTypes type)
        {
            try
            {
                await _connection.DeleteAsync(type);
                return type;
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting type: {e.Message}");
                return null;
            }
        }

        public bool DoesTypeNameExist(string typeName)
        {
            throw new NotImplementedException();
        }

        public string GetNameOfTypeByID(int typeId)
        {
            throw new NotImplementedException();
        }

        public bool IsTypeUsedAndCannotBeDeleted(int typeId)
        {
            throw new NotImplementedException();
        }

        // private methods

        private async Task InitializeAsync()
        {
            if (_connection != null)
            {
                return;
            }

            _connection = new SQLiteAsyncConnection(_dbPath);
            await _connection.CreateTableAsync<BankAccountTypes>();

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
