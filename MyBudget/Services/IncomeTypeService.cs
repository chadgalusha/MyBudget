using MyBudget.DataAccess;
using MyBudget.Models;
using Serilog;

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
                Log.Error($"Error creating new income type: {e.Message}");
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
                Log.Error($"Error updating income type: {e.Message}");
                return new IncomeTypes() { IncomeTypeId = 0 };
            }
        }

        public async Task<IncomeTypes> DeleteRecord(IncomeTypes type)
        {
            if (IsIncomeTypeUsedByIncome(type.IncomeTypeId) == true)
            {
                return new IncomeTypes() { IncomeTypeId = -1 };
            }

            try
            {
                return await _incomeTypeDataAccess.DeleteRecordAsync(type);
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting income type: {e.Message}");
                return new IncomeTypes() { IncomeTypeId = 0 };
            }
        }

        // private methods

        private bool IsIncomeTypeNameAlreadyUsed(string incomeTypeName)
        {
            return _incomeTypeDataAccess.DoesTypeNameExist(incomeTypeName);
        }

        private bool IsUpdatedIncomeTypeNameModified(IncomeTypes incomeType)
        {
            string currentIncomeTypeName = _incomeTypeDataAccess.GetNameOfTypeByID(incomeType.IncomeTypeId);
            return currentIncomeTypeName.Equals(incomeType.IncomeType);
        }

        private bool IsIncomeTypeUsedByIncome(int incomeTypeId)
        {
            return _incomeTypeDataAccess.IsTypeUsedAndCannotBeDeleted(incomeTypeId);
        }
    }
}
