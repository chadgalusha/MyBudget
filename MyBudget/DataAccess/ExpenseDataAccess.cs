using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class ExpenseDataAccess : IExpenseDataAccess
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public ExpenseDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task<Expenses> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<Expenses>()
                .Where(i => i.ExpensesId == id)
                .FirstAsync();
        }

        public async Task<List<Expenses>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<Expenses>().ToListAsync();
        }

        public async Task<Expenses> CreateRecord(Expenses newExpense)
        {
            try
            {
                await _asyncConnection.InsertAsync(newExpense).ContinueWith((e) =>
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
                await _asyncConnection.UpdateAsync(expense).ContinueWith((e) =>
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
                await _asyncConnection.DeleteAsync(expense).ContinueWith((e) =>
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

        public bool DoesExpenseNameExist(string expenseName)
        {
            int result;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                result = _connection.Table<Expenses>()
                    .Where(e => e.ExpensesName.ToLower() == expenseName.ToLower())
                    .Count();
            }

            return result > 0;
        }

        public string GetNameOfExpenseById(int id)
        {
            string expenseName;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                expenseName = _connection.Table<Expenses>()
                    .Where(e => e.ExpensesId == id)
                    .Select(e => e.ExpensesName)
                    .First();
            }

            return expenseName;
        }

        // private methods

        private async Task InitializeAsync()
        {
            if (_asyncConnection != null)
            {
                return;
            }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
            await _asyncConnection.CreateTableAsync<Expenses>().ContinueWith((results) =>
            {
                Log.Information($"Expenses table created: {results.Result}");
            });
        }
    }
}
