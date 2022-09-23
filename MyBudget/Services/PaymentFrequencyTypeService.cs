using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;
using Serilog;
using SQLite;

namespace MyBudget.Services
{
    public class PaymentFrequencyTypeService
    {
        private readonly PaymentFrequencyTypeDataAccess _paymentFrequencyTypeDataAccess;

        public PaymentFrequencyTypeService(PaymentFrequencyTypeDataAccess paymentFrequencyTypeDataAccess)
        {
            _paymentFrequencyTypeDataAccess = paymentFrequencyTypeDataAccess;
        }

        public async Task<PaymentFrequencyTypes> GetById(int id)
        {
            return await _paymentFrequencyTypeDataAccess.GetRecordByIdAsync(id);
        }

        public async Task<List<PaymentFrequencyTypes>> GetList()
        {
            return await _paymentFrequencyTypeDataAccess.GetListAsync();
        }

        public async Task<PaymentFrequencyTypes> CreateRecord(PaymentFrequencyTypes newType)
        {
            return await _paymentFrequencyTypeDataAccess.CreateRecord(newType);
        }

        public async Task<PaymentFrequencyTypes> UpdateRecord(PaymentFrequencyTypes type)
        {
            return await _paymentFrequencyTypeDataAccess.UpdateRecordAsync(type);
        }

        public async Task<PaymentFrequencyTypes> DeleteRecord(PaymentFrequencyTypes type)
        {
            return await _paymentFrequencyTypeDataAccess.DeleteRecordAsync(type);
        }
    }
}
