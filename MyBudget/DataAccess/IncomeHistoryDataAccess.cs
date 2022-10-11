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
                    MyBudgetLogger.CreatedLogMessage(newIncomeHistory);
                });
                return newIncomeHistory;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newIncomeHistory, e);
                return new IncomeHistory() { IncomeHistoryId = 0 };
            }
        }

        public async Task<IncomeHistory> UpdateRecordAsync(IncomeHistory incomehistory)
        {
            try
            {
                await _asyncConnection.UpdateAsync(incomehistory).ContinueWith((i) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(incomehistory);
                });
                
                return incomehistory;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(incomehistory, e);
                return new IncomeHistory() { IncomeHistoryId = 0 };
            }
        }

        public async Task<IncomeHistory> DeleteRecordAsync(IncomeHistory incomehistory)
        {
            try
            {
                await _asyncConnection.DeleteAsync(incomehistory).ContinueWith((i) =>
                {
                    MyBudgetLogger.DeletedLogMessage(incomehistory);
                });
                
                return incomehistory;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(incomehistory, e);
                return new IncomeHistory() { IncomeHistoryId = 0 };
            }
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
