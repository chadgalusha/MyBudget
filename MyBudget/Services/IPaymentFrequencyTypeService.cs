using MyBudget.Models;

namespace MyBudget.Services
{
    public interface IPaymentFrequencyTypeService
    {
        Task<PaymentFrequencyTypes> CreateRecord(PaymentFrequencyTypes newType);
        Task<PaymentFrequencyTypes> DeleteRecord(PaymentFrequencyTypes type);
        Task<PaymentFrequencyTypes> GetById(int id);
        Task<List<PaymentFrequencyTypes>> GetList();
        Task<PaymentFrequencyTypes> UpdateRecord(PaymentFrequencyTypes type);
    }
}