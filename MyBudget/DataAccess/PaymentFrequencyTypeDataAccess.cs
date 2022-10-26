using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
    public class PaymentFrequencyTypeDataAccess : ITypeDataAccess<PaymentFrequencyTypes>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _asyncConnection;
        private SQLiteConnection _connection;

        public PaymentFrequencyTypeDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<PaymentFrequencyTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _asyncConnection.Table<PaymentFrequencyTypes>()
                .Where(p => p.PaymentFrequencyTypeId == id)
                .FirstAsync();
        }

        public async Task<List<PaymentFrequencyTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _asyncConnection.Table<PaymentFrequencyTypes>().ToListAsync();
        }

        public async Task<PaymentFrequencyTypes> CreateRecord(PaymentFrequencyTypes newType)
		{
            try
            {
                await _asyncConnection.InsertAsync(newType).ContinueWith((p) =>
                {
                    MyBudgetLogger.CreatedLogMessage(newType);
                });
                return newType;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newType, e);
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = 0 };
            }
        }

        public async Task<PaymentFrequencyTypes> UpdateRecordAsync(PaymentFrequencyTypes type)
        {
            try
            {
                await _asyncConnection.UpdateAsync(type).ContinueWith((p) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(type);
                });
                return type;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(type, e);
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = 0 };
            }
        }

        public async Task<PaymentFrequencyTypes> DeleteRecordAsync(PaymentFrequencyTypes type)
		{
            try
            {
                await _asyncConnection.DeleteAsync(type).ContinueWith((p) =>
                {
                    MyBudgetLogger.DeletedLogMessage(type);
                });
                return type;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(type, e);
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = 0 };
            }
        }

        public bool DoesTypeNameExist(string typeName)
        {
            using (_connection = new SQLiteConnection(_dbPath))
            {
                int result = _connection.Table<PaymentFrequencyTypes>()
                    .Where(p => p.PaymentFrequencyType.ToLower() == typeName.ToLower())
                    .Count();

                return result > 0;
            }
        }

        public string GetNameOfTypeByID(int typeId)
        {
            using (_connection = new SQLiteConnection(_dbPath))
            {
                string paymentFrequencyTypeName = _connection.Table<PaymentFrequencyTypes>()
                    .Where(p => p.PaymentFrequencyTypeId == typeId)
                    .Select(p => p.PaymentFrequencyType)
                    .SingleOrDefault();

                return paymentFrequencyTypeName;
            }
        }

        public bool IsTypeUsedAndCannotBeDeleted(int typeId)
        {
            using (_connection = new SQLiteConnection(_dbPath))
            {
                int resultIncomes = _connection.Table<Incomes>()
                    .Where(p => p.PaymentFrequencyTypeId == typeId)
                    .Count();

                int resultExpenses = _connection.Table<Expenses>()
                    .Where(p => p.PaymentFrequencyTypeId == typeId)
                    .Count();

                return resultIncomes > 0 || resultExpenses > 0;
            }
        }

        // private methods

        private async Task InitializeAsync()
        {
            if (_asyncConnection != null)
            {
                return;
            }

            _asyncConnection = new SQLiteAsyncConnection(_dbPath);
            //await _connection.CreateTableAsync<PaymentFrequencyTypes>();

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
            var initialValuesArray = PaymentFrequencyTypes.InitialValues();

            foreach (var value in initialValuesArray)
            {
                PaymentFrequencyTypes newType = new()
                {
                    PaymentFrequencyType = value
                };

                await CreateRecord(newType);
            }
        }
    }
}
