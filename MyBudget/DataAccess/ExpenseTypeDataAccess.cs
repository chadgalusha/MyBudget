using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
    public class ExpenseTypeDataAccess : ITypeDataAccess<ExpenseTypes>
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public ExpenseTypeDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<ExpenseTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<ExpenseTypes>()
                .Where(e => e.ExpenseTypeId == id)
                .FirstAsync();
        }

        public async Task<List<ExpenseTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<ExpenseTypes>().ToListAsync();
        }

        public async Task<ExpenseTypes> CreateRecord(ExpenseTypes newType)
        {
            try
            {
                await _asyncConnection.InsertAsync(newType);
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
                await _asyncConnection.UpdateAsync(type);
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
                await _asyncConnection.DeleteAsync(type);
                return type;
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting type: {e.Message}");
                return null;
            }
        }

        public bool DoesTypeNameExist(string expenseTypeName)
        {
            int result;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                result = _connection.Table<ExpenseTypes>()
                    .Where(e => e.ExpenseType.ToLower() == expenseTypeName.ToLower())
                    .Count();
            }

            return result > 0;
        }

        public string GetNameOfTypeByID(int id)
        {
            string expenseTypeName;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                expenseTypeName = _connection.Table<ExpenseTypes>()
                    .Where(e => e.ExpenseTypeId == id)
                    .Select(e => e.ExpenseType)
                    .SingleOrDefault();
            }

            return expenseTypeName;
        }

        public bool IsTypeUsedAndCannotBeDeleted(int expenseTypeId)
        {
            int result;
            using (_connection = new SQLiteConnection(_dbPath))
            {
                result = _connection.Table<Expenses>()
                    .Where(e => e.ExpenseTypeId == expenseTypeId)
                    .Count();
            }

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
            //await _asyncConnection.CreateTableAsync<ExpenseTypes>();

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
