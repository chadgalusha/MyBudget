using MyBudget.DataAccess;
using MyBudget.Models;
using Serilog;

namespace MyBudget.Services
{
    public class ExpenseTypeService : ITypeService<ExpenseTypes>
    {
        private readonly ITypeDataAccess<ExpenseTypes> _expenseTypeDataAccess;

        public ExpenseTypeService(ITypeDataAccess<ExpenseTypes> expenseTypeDataAccess)
        {
            _expenseTypeDataAccess = expenseTypeDataAccess;
        }

        public async Task<ExpenseTypes> GetById(int id)
        {
            return await _expenseTypeDataAccess.GetRecordByIdAsync(id);
        }

        public async Task<List<ExpenseTypes>> GetList()
        {
            return await _expenseTypeDataAccess.GetListAsync();
        }

        public async Task<ExpenseTypes> CreateRecord(ExpenseTypes newType)
        {
            if (IsExpenseTypeNameAlreadyUsed(newType.ExpenseType) == true)
            {
                return new ExpenseTypes() { ExpenseTypeId = -1 };
            }

            try
            {
                ExpenseTypes newExpenseType = new()
                {
                    ExpenseType = newType.ExpenseType
                };

                return await _expenseTypeDataAccess.CreateRecord(newExpenseType);
            }
            catch (Exception e)
            {
                Log.Error($"Error creating new expense type: {e.Message}");
                return new ExpenseTypes() { ExpenseTypeId = 0 };
            }
        }

        public async Task<ExpenseTypes> UpdateRecord(ExpenseTypes expenseType)
        {
            if (IsUpdatedExpenseTypeNameModified(expenseType) == true)
            {
                if (IsExpenseTypeNameAlreadyUsed(expenseType.ExpenseType) == true)
                {
                    return new ExpenseTypes() { ExpenseTypeId = -1 };
                }
            }

            try
            {
                return await _expenseTypeDataAccess.UpdateRecordAsync(expenseType);
            }
            catch (Exception e)
            {
                Log.Error($"Error updating expense type: {e.Message}");
                return new ExpenseTypes() { ExpenseTypeId = 0 };
            }
        }

        public async Task<ExpenseTypes> DeleteRecord(ExpenseTypes type)
        {
            if (IsExpenseTypeUsedByExpense(type.ExpenseTypeId) == true)
            {
                return new ExpenseTypes() { ExpenseTypeId = -1 };
            }

            try
            {
                return await _expenseTypeDataAccess.DeleteRecordAsync(type);
            }
            catch (Exception e)
            {
                Log.Error($"Error deleting expense type: {e.Message}");
                return new ExpenseTypes() { ExpenseTypeId = 0 };
            }
        }

        #region Private Methods

        private bool IsExpenseTypeNameAlreadyUsed(string expenseTypeName)
        {
            return _expenseTypeDataAccess.DoesTypeNameExist(expenseTypeName);
        }

        // return false if NOT modified
        private bool IsUpdatedExpenseTypeNameModified(ExpenseTypes expenseType)
        {
            string currentExpenseTypeName = _expenseTypeDataAccess.GetNameOfTypeByID(expenseType.ExpenseTypeId);
            return !currentExpenseTypeName.Equals(expenseType.ExpenseType);
        }

        private bool IsExpenseTypeUsedByExpense(int expenseTypeId)
        {
            return _expenseTypeDataAccess.IsTypeUsedAndCannotBeDeleted(expenseTypeId);
        }

        #endregion
    }
}
