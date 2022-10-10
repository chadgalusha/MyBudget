using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class IncomeHistoryDataAccess : IHistoryDataAccess<IncomeHistory>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public IncomeHistoryDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<IncomeHistory> GetRecordByIdAsync(int id)
        {
            Initialize();
            return await _asyncConnection.Table<IncomeHistory>()
                .Where(i => i.IncomeHistoryId == id)
                .FirstAsync();
        }

        public async Task<List<IncomeHistory>> GetListAsync()
        {
            Initialize();

            return await _asyncConnection.Table<IncomeHistory>().ToListAsync();
        }

        public async Task<IncomeHistory> CreateRecordAsync(IncomeHistory newIncomeHistory)
        {
            try
            {
                await _asyncConnection.InsertAsync(newIncomeHistory).ContinueWith((i) =>
                {
                    Log.Information($"New income history created: [Name: {newIncomeHistory.IncomeName}] [Date: {newIncomeHistory.IncomeDate}] [Amount: {newIncomeHistory.IncomeAmount}]");
                });
                return newIncomeHistory;
            }
            catch (Exception e)
            {
                Log.Error($"Error creating new income history: {e.Message}");
                return new IncomeHistory() { IncomeHistoryId = -1 };
            }
        }

        public async Task<IncomeHistory> UpdateRecordAsync(IncomeHistory incomehistory)
        {
            throw new NotImplementedException();
        }

        public async Task<IncomeHistory> DeleteRecordAsync(IncomeHistory incomehistory)
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        private void Initialize()
        {
            if (_asyncConnection != null)
            {
                return;
            }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
        }

        #endregion
    }
}
