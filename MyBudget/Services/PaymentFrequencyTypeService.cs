using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class PaymentFrequencyTypeService : ITypeService<PaymentFrequencyTypes>
    {
        private readonly ITypeDataAccess<PaymentFrequencyTypes> _paymentFrequencyTypeDataAccess;

        public PaymentFrequencyTypeService(ITypeDataAccess<PaymentFrequencyTypes> paymentFrequencyTypeDataAccess)
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
            if (IsPaymentFrequencyNameAlreadyUsed(newType.PaymentFrequencyType) == true)
            {
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = -1 };
            }

            try
            {
                PaymentFrequencyTypes newPaymentFrequencyType = new()
                {
                    PaymentFrequencyType = newType.PaymentFrequencyType
                };

                return await _paymentFrequencyTypeDataAccess.CreateRecord(newPaymentFrequencyType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newType, e);
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = 0 };
            }
        }

        public async Task<PaymentFrequencyTypes> UpdateRecord(PaymentFrequencyTypes paymentFrequencyType)
        {
            if (IsUpdatedPaymentFrequencyNameModified(paymentFrequencyType) == true)
            {
                if (IsPaymentFrequencyNameAlreadyUsed(paymentFrequencyType.PaymentFrequencyType) == true)
                {
                    return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = -1 };
                }
            }

            try
            {
                return await _paymentFrequencyTypeDataAccess.UpdateRecordAsync(paymentFrequencyType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(paymentFrequencyType, e);
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = 0 };
            }
        }

        public async Task<PaymentFrequencyTypes> DeleteRecord(PaymentFrequencyTypes paymentFrequencyType)
        {
            if (IsPaymentFrequencyUsedByIncomeOrExpenses(paymentFrequencyType.PaymentFrequencyTypeId) == true)
            {
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = -1 };
            }

            try
            {
                return await _paymentFrequencyTypeDataAccess.DeleteRecordAsync(paymentFrequencyType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(paymentFrequencyType, e);
                return new PaymentFrequencyTypes() { PaymentFrequencyTypeId = 0 };
            }
        }

        // PRIVATE METHODS

        private bool IsPaymentFrequencyNameAlreadyUsed(string paymentFrequencyName)
        {
            return _paymentFrequencyTypeDataAccess.DoesTypeNameExist(paymentFrequencyName);
        }

        private bool IsUpdatedPaymentFrequencyNameModified(PaymentFrequencyTypes paymentFrequencyType)
        {
            string currentPaymentFrequencyName = _paymentFrequencyTypeDataAccess.GetNameOfTypeByID(paymentFrequencyType.PaymentFrequencyTypeId);
            return !currentPaymentFrequencyName.Equals(paymentFrequencyType.PaymentFrequencyType);
        }

        private bool IsPaymentFrequencyUsedByIncomeOrExpenses(int paymentFrequencyTypeId)
        {
            return _paymentFrequencyTypeDataAccess.IsTypeUsedAndCannotBeDeleted(paymentFrequencyTypeId);
        }
    }
}
