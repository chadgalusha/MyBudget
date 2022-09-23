using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.DataAccess
{
	public class PaymentFrequencyTypeDataAccess : IDataAccess<PaymentFrequencyTypes>
	{
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public PaymentFrequencyTypeDataAccess()
        {
            _dbPath = DatbasePath.GetDbPath();
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
                await _connection.InsertAsync(newType);
                return newType;
            }
            catch (Exception e)
            {
                Log.Error($"Error inserting new record: {e.Message}");
                return null;
            }
        }

        public async Task<PaymentFrequencyTypes> UpdateRecordAsync(PaymentFrequencyTypes type)
        {
            try
            {
                await _connection.UpdateAsync(type);
                return type;
            }
            catch (Exception e)
            {
                Log.Error($"Error updating record: {e.Message}");
                return null;
            }
        }

        public async Task<PaymentFrequencyTypes> DeleteRecordAsync(PaymentFrequencyTypes type)
		{
            try
            {
                await _connection.DeleteAsync(type);
                return type;
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting type: {e.Message}");
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
            await _connection.CreateTableAsync<PaymentFrequencyTypes>();

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
