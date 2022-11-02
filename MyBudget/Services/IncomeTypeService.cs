using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class IncomeTypeService : ITypeService<IncomeTypes>
    {
        private readonly ITypeDataAccess<IncomeTypes> _incomeTypeDataAccess;

        public IncomeTypeService(ITypeDataAccess<IncomeTypes> incomeTypeDataAccess)
        {
            _incomeTypeDataAccess = incomeTypeDataAccess;
        }

        public async Task<IncomeTypes> GetById(int id)
        {
            return await _incomeTypeDataAccess.GetRecordByIdAsync(id);
        }

        public async Task<List<IncomeTypes>> GetList()
        {
            return await _incomeTypeDataAccess.GetListAsync();
        }

        public async Task<IncomeTypes> CreateRecord(IncomeTypes newType)
        {
            if (IsIncomeTypeNameAlreadyUsed(newType.IncomeType) == true)
            {
                return new IncomeTypes() { IncomeTypeId = -1 };
            }

            try
            {
                IncomeTypes newIncomeType = new()
                {
                    IncomeType = newType.IncomeType
                };

                return await _incomeTypeDataAccess.CreateRecord(newIncomeType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newType, e);
                return new IncomeTypes() { IncomeTypeId = 0 };
            }
        }

        public async Task<IncomeTypes> UpdateRecord(IncomeTypes incomeType)
        {
            if (IsUpdatedIncomeTypeNameModified(incomeType) == true)
            {
                if (IsIncomeTypeNameAlreadyUsed(incomeType.IncomeType) == true)
                {
                    return new IncomeTypes() { IncomeTypeId = -1 };
                }
            }

            try
            {
                return await _incomeTypeDataAccess.UpdateRecordAsync(incomeType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(incomeType, e);
                return new IncomeTypes() { IncomeTypeId = 0 };
            }
        }

        public async Task<IncomeTypes> DeleteRecord(IncomeTypes incomeType)
        {
            if (IsIncomeTypeUsedByIncome(incomeType.IncomeTypeId) == true)
            {
                return new IncomeTypes() { IncomeTypeId = -1 };
            }

            try
            {
                return await _incomeTypeDataAccess.DeleteRecordAsync(incomeType);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(incomeType, e);
                return new IncomeTypes() { IncomeTypeId = 0 };
            }
        }

        #region Private Methods

        private bool IsIncomeTypeNameAlreadyUsed(string incomeTypeName)
        {
            return _incomeTypeDataAccess.DoesTypeNameExist(incomeTypeName);
        }

        // return false if NOT modified
        private bool IsUpdatedIncomeTypeNameModified(IncomeTypes incomeType)
        {
            string currentIncomeTypeName = _incomeTypeDataAccess.GetNameOfTypeByID(incomeType.IncomeTypeId);
            return !currentIncomeTypeName.Equals(incomeType.IncomeType);
        }

        private bool IsIncomeTypeUsedByIncome(int incomeTypeId)
        {
            return _incomeTypeDataAccess.IsTypeUsedAndCannotBeDeleted(incomeTypeId);
        }

        #endregion
    }
}
