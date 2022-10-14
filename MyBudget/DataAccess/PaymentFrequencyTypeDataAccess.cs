using MyBudget.Helpers;
using MyBudget.Models;
using SQLite;

namespace MyBudget.DataAccess
{
    public class PaymentFrequencyTypeDataAccess : ITypeDataAccess<PaymentFrequencyTypes>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public PaymentFrequencyTypeDataAccess()
        {
            _dbPath = DatabaseHelper.GetDbPath();
        }

        public async Task<PaymentFrequencyTypes> GetRecordByIdAsync(int id)
        {
            await InitializeAsync();
            return await _connection.Table<PaymentFrequencyTypes>()
                .Where(p => p.PaymentFrequencyTypeId == id)
                .FirstAsync();
        }

        public async Task<List<PaymentFrequencyTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<PaymentFrequencyTypes>().ToListAsync();
        }

        public async Task<PaymentFrequencyTypes> CreateRecord(PaymentFrequencyTypes newType)
		{
            try
            {
                await _connection.InsertAsync(newType).ContinueWith((p) =>
                {
                    MyBudgetLogger.CreatedLogMessage(newType);
                });
                return newType;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newType, e);
                return null;
            }
        }

        public async Task<PaymentFrequencyTypes> UpdateRecordAsync(PaymentFrequencyTypes type)
        {
            try
            {
                await _connection.UpdateAsync(type).ContinueWith((p) =>
                {
                    MyBudgetLogger.UpdatedLogMessage(type);
                });
                return type;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(type, e);
                return null;
            }
        }

        public async Task<PaymentFrequencyTypes> DeleteRecordAsync(PaymentFrequencyTypes type)
		{
            try
            {
                await _connection.DeleteAsync(type).ContinueWith((p) =>
                {
                    MyBudgetLogger.DeletedLogMessage(type);
                });
                return type;
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(type, e);
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

        public bool DoesTypeNameExist(string typeName)
        {
            throw new NotImplementedException();
        }

        public string GetNameOfTypeByID(int typeId)
        {
            throw new NotImplementedException();
        }

        public bool IsTypeUsedAndCannotBeDeleted(int typeId)
        {
            throw new NotImplementedException();
        }
    }
}
