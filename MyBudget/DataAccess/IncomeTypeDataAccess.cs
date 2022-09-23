using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class IncomeTypeDataAccess : IDataAccess<IncomeTypes>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public IncomeTypeDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<IncomeTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _connection.Table<IncomeTypes>()
                .Where(i => i.IncomeTypeId == id)
                .FirstAsync();
        }

        public async Task<List<IncomeTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<IncomeTypes>().ToListAsync();
        }

        public async Task<IncomeTypes> CreateRecord(IncomeTypes newType)
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

        public async Task<IncomeTypes> UpdateRecordAsync(IncomeTypes type)
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

        public async Task<IncomeTypes> DeleteRecordAsync(IncomeTypes type)
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
            await _connection.CreateTableAsync<IncomeTypes>();

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
            var initialValuesArray = IncomeTypes.InitialValues();

            foreach (var value in initialValuesArray)
            {
                IncomeTypes newType = new()
                {
                    IncomeType = value
                };

                await CreateRecord(newType);
            }
        }
    }
}
