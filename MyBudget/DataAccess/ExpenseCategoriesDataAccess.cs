using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
    public class ExpenseCategoriesDataAccess : IDataAccess<ExpenseCategories>
    {
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;
        private readonly string _dbPath;

        public ExpenseCategoriesDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<ExpenseCategories> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<ExpenseCategories>()
                .Where(e => e.ExpenseCategoryId == id)
                .FirstAsync();
        }

        public async Task<List<ExpenseCategories>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<ExpenseCategories>().ToListAsync();
        }

        public async Task<ExpenseCategories> CreateRecord(ExpenseCategories newExpenseCategory)
        {
            try
            {
                await _asyncConnection.InsertAsync(newExpenseCategory).ContinueWith((e) =>
                {
                    MyBudgetLogger.CreatedLogMessage(newExpenseCategory);
                });
                return newExpenseCategory;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newExpenseCategory, e);
                return new ExpenseCategories() { ExpenseCategoryId = 0 };
            }
        }

        public async Task<ExpenseCategories> UpdateRecordAsync(ExpenseCategories expenseCategory)
        {
            try
            {
                await _asyncConnection.UpdateAsync(expenseCategory).ContinueWith((e) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(expenseCategory);
                });
                return expenseCategory;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(expenseCategory, e);
                return new ExpenseCategories() { ExpenseCategoryId = 0 };
            }
        }

        public async Task<ExpenseCategories> DeleteRecordAsync(ExpenseCategories expenseCategory)
        {
            try
            {
                await _asyncConnection.DeleteAsync(expenseCategory).ContinueWith((e) => 
                { 
                    MyBudgetLogger.DeletedLogMessage(expenseCategory); 
                });
                return expenseCategory;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(expenseCategory, e);
                return new ExpenseCategories() { ExpenseCategoryId = 0 };
            }
        }

        public bool DoesNameExist(string name)
        {
            int result;

            using (_connection = new SQLiteConnection(_dbPath))
            {
                result = _connection.Table<ExpenseCategories>()
                    .Where(e => e.ExpenseCategoryName.ToLower() == name.ToLower())
                    .Count();
            }

            return result > 0;
        }

        public string GetNameById(int id)
        {
            string expenseCategoryName;

            using (_connection = new SQLiteConnection(_dbPath))
            {
                expenseCategoryName = _connection.Table<ExpenseCategories>()
                        .Where(e => e.ExpenseCategoryId == id)
                        .Select(e => e.ExpenseCategoryName)
                        .SingleOrDefault();
            }

            return expenseCategoryName;
        }

        // PRIVATE METHODS

        private async Task InitializeAsync()
        {
            if (_asyncConnection != null) { return; }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);

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
            Dictionary<string, string> initialValuesDictionary = ExpenseCategories.InitialValues();

            foreach (var keyValuePair in initialValuesDictionary)
            {
                ExpenseCategories newExpenseCategory = new()
                {
                    ExpenseCategoryName = keyValuePair.Key,
                    ExpenseCategoryDescription = keyValuePair.Value
                };

                await CreateRecord(newExpenseCategory);
            }
        }
    }
}
