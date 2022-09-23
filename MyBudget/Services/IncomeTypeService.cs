using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class IncomeTypeService
    {
        private readonly IncomeTypeDataAccess _incomeTypeDataAccess;

        public IncomeTypeService(IncomeTypeDataAccess incomeTypeDataAccess)
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
            return await _incomeTypeDataAccess.CreateRecord(newType);
        }

        public async Task<IncomeTypes> UpdateRecord(IncomeTypes type)
        {
            return await _incomeTypeDataAccess.UpdateRecordAsync(type);
        }

        public async Task<IncomeTypes> DeleteRecord(IncomeTypes type)
        {
            return await _incomeTypeDataAccess.DeleteRecordAsync(type);
        }
    }
}
