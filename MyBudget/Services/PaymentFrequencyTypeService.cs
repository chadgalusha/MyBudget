using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.Services
{
    public class PaymentFrequencyTypeService
    {
        private readonly string _dbPath;
        private SQLiteAsyncConnection _connection;

        public PaymentFrequencyTypeService()
        {
            _dbPath = DatbasePath.GetDbPath();
        }

        public async Task InitializeAsync()
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

        public async  Task<List<PaymentFrequencyTypes>> GetListAsync()
        {
            await InitializeAsync();
            return await _connection.Table<PaymentFrequencyTypes>().ToListAsync();
        }

        public async Task<PaymentFrequencyTypes> CreatePaymentFrequencyTypeAsync(PaymentFrequencyTypes newType)
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

        public async Task<PaymentFrequencyTypes> UpdateTypeAsync(PaymentFrequencyTypes type)
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

        public async Task<PaymentFrequencyTypes> DeleteTypeAsync(PaymentFrequencyTypes type)
        {
            try
            {
                await _connection.DeleteAsync(type);
                return type;
            }
            catch (Exception e)
            {
                Log.Error($"Error delting type: {e.Message}");
                return null;
            }
        }

        // private methods

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

                await CreatePaymentFrequencyTypeAsync(newType);
            }
        }
    }
}
