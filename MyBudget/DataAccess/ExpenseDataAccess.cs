using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
    public class ExpenseDataAccess : IDataAccess<Expenses>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public ExpenseDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<Expenses> GetRecordByIdAsync(int id)
        {
            Initialize();
            return await _asyncConnection.Table<Expenses>()
                .Where(i => i.ExpensesId == id)
                .FirstAsync();
        }

        public async Task<List<Expenses>> GetListAsync()
        {
            Initialize();
            return await _asyncConnection.Table<Expenses>().ToListAsync();
        }

        public async Task<Expenses> CreateRecord(Expenses newExpense)
        {
            try
            {
                await _asyncConnection.InsertAsync(newExpense).ContinueWith((e) =>
                {
                    MyBudgetLogger.CreatedLogMessage(newExpense);
                });
                return newExpense;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newExpense, e);
                return null;
            }
        }

        public async Task<Expenses> UpdateRecordAsync(Expenses expense)
        {
            try
            {
                await _asyncConnection.UpdateAsync(expense).ContinueWith((e) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(expense);
                });
                return expense;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(expense, e);
                return null;
            }
        }

        public async Task<Expenses> DeleteRecordAsync(Expenses expense)
        {
            try
            {
                await _asyncConnection.DeleteAsync(expense).ContinueWith((e) =>
                {
                    MyBudgetLogger.DeletedLogMessage(expense);
                });
                return expense;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(expense, e);
                return null;
            }
        }

        public bool DoesNameExist(string expenseName)
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

        public string GetNameById(int id)
        {
            string expenseName;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                expenseName = _connection.Table<Expenses>()
                    .Where(e => e.ExpensesId == id)
                    .Select(e => e.ExpensesName)
                    .SingleOrDefault();
            }

            return expenseName;
        }

        // private methods

        private void Initialize()
        {
            if (_asyncConnection != null) { return; }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
        }
    }
}
