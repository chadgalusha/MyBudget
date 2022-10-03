using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class PaymentFrequencyTypeService : IPaymentFrequencyTypeService
    {
        private readonly IDataAccess<PaymentFrequencyTypes> _paymentFrequencyTypeDataAccess;

        public PaymentFrequencyTypeService(IDataAccess<PaymentFrequencyTypes> paymentFrequencyTypeDataAccess)
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
