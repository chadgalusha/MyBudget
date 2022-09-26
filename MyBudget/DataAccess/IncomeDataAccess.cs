using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class IncomeDataAccess : IDataAccess<Incomes>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public IncomeDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<Incomes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<Incomes>()
                .Where(i => i.IncomeId == id)
                .FirstAsync();
        }

        public async Task<List<Incomes>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<Incomes>().ToListAsync();
        }

        public async Task<Incomes> CreateRecord(Incomes newIncome)
        {
            try
            {
                await _asyncConnection.InsertAsync(newIncome).ContinueWith((i) =>
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
                await _asyncConnection.UpdateAsync(income).ContinueWith((i) =>
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
                await _asyncConnection.DeleteAsync(income).ContinueWith((i) =>
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

        public bool DoesIncomeNameExist(string incomeName)
        {
            var result = _connection.Table<Incomes>()
                .Where(i => i.IncomeName.ToLower() == incomeName.ToLower())
                .Count();

            return result > 0;
        }

        // private methods

        private async Task InitializeAsync()
        {
            if (_asyncConnection != null)
            {
                return;
            }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
            await _asyncConnection.CreateTableAsync<Incomes>().ContinueWith((results) =>
            {
                Log.Information($"Incomes table created: {results.Result}");
            });
        }
    }
}
