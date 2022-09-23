using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class ExpenseDataAccess
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public ExpenseDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<Expenses> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _connection.Table<Expenses>()
                .Where(i => i.ExpensesId == id)
                .FirstAsync();
        }

        public async Task<List<Expenses>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<Expenses>().ToListAsync();
        }

        public async Task<Expenses> CreateRecord(Expenses newExpense)
        {
            try
            {
                await _connection.InsertAsync(newExpense).ContinueWith((e) =>
                {
                    Log.Information($"Expense created: {newExpense.ExpensesName}");
                });
                return newExpense;
            }
            catch (Exception e)
            {
                Log.Error($"Error inserting new expense: {e.Message}");
                return null;
            }
        }

        public async Task<Expenses> UpdateRecordAsync(Expenses expense)
        {
            try
            {
                await _connection.UpdateAsync(expense).ContinueWith((e) =>
                {
                    Log.Information($"Expense updated: {expense.ExpensesId}, {expense.ExpensesName}");
                });
                return expense;
            }
            catch (Exception e)
            {
                Log.Error($"Error updating expense: {e.Message}");
                return null;
            }
        }

        public async Task<Expenses> DeleteRecordAsync(Expenses expense)
        {
            try
            {
                await _connection.DeleteAsync(expense).ContinueWith((e) =>
                {
                    Log.Information($"Expense deleted: {expense.ExpensesId}, {expense.ExpensesName}");
                });
                return expense;
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting expense: {e.Message}");
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
            await _connection.CreateTableAsync<Expenses>().ContinueWith((results) =>
            {
                Log.Information($"Expenses table created: {results.Result}");
            });
        }
    }
}
