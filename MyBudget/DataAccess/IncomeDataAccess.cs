using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class IncomeDataAccess : IDataAccess<Incomes>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public IncomeDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<Incomes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _connection.Table<Incomes>()
                .Where(i => i.IncomeId == id)
                .FirstAsync();
        }

        public async Task<List<Incomes>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<Incomes>().ToListAsync();
        }

        public async Task<Incomes> CreateRecord(Incomes newIncome)
        {
            try
            {
                await _connection.InsertAsync(newIncome).ContinueWith((i) =>
                {
                    Log.Information($"Income created: {newIncome.IncomeName}");
                });
                return newIncome;
            }
            catch (Exception e)
            {
                Log.Error($"Error inserting new income: {e.Message}");
                return null;
            }
        }

        public async Task<Incomes> UpdateRecordAsync(Incomes income)
        {
            try
            {
                await _connection.UpdateAsync(income).ContinueWith((i) =>
                {
                    Log.Information($"Income updated: {income.IncomeId}, {income.IncomeName}");
                });
                return income;
            }
            catch (Exception e)
            {
                Log.Error($"Error updating income: {e.Message}");
                return null;
            }
        }

        public async Task<Incomes> DeleteRecordAsync(Incomes income)
        {
            try
            {
                await _connection.DeleteAsync(income).ContinueWith((i) =>
                {
                    Log.Information($"Income deleted: {income.IncomeId}, {income.IncomeName}");
                });
                return income;
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting income: {e.Message}");
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
            await _connection.CreateTableAsync<Incomes>().ContinueWith((results) =>
            {
                Log.Information($"Expenses table created: {results.Result}");
            });
        }
    }
}
