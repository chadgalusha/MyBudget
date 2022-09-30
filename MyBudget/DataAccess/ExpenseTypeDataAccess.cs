using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class ExpenseTypeDataAccess : IDataAccess<ExpenseTypes>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public ExpenseTypeDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<ExpenseTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _connection.Table<ExpenseTypes>()
                .Where(e => e.ExpenseTypeId == id)
                .FirstAsync();
        }

        public async Task<List<ExpenseTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<ExpenseTypes>().ToListAsync();
        }

        public async Task<ExpenseTypes> CreateRecord(ExpenseTypes newType)
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

        public async Task<ExpenseTypes> UpdateRecordAsync(ExpenseTypes type)
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

        public async Task<ExpenseTypes> DeleteRecordAsync(ExpenseTypes type)
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

        // private methods

        private async Task InitializeAsync()
        {
            if (_connection != null)
            {
                return;
            }

            _connection = new SQLiteAsyncConnection(_dbPath);
            await _connection.CreateTableAsync<ExpenseTypes>();

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
            var initialValuesArray = ExpenseTypes.InitialValues();

            foreach (var value in initialValuesArray)
            {
                ExpenseTypes newType = new()
                {
                    ExpenseType = value
                };

                await CreateRecord(newType);
            }
        }
    }
}
// initial change.