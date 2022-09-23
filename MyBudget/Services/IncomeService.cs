using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
	public class IncomeService
	{
		private readonly IncomeDataAccess _incomeDataAccess;

		public IncomeService(IncomeDataAccess incomeDataAccess)
		{
			_incomeDataAccess = incomeDataAccess;
		}

		public async Task<Incomes> GetById(int id)
		{
			return await _incomeDataAccess.GetRecordByIdAsync(id);
		}

		public async Task<List<Incomes>> GetList()
		{
			return await _incomeDataAccess.GetListAsync();
		}

        public async Task<Incomes> CreateRecord(Incomes newIncome)
        {
            return await _incomeDataAccess.CreateRecord(newIncome);
        }

        public async Task<Incomes> UpdateRecord(Incomes income)
        {
            return await _incomeDataAccess.UpdateRecordAsync(income);
        }

        public async Task<Incomes> DeleteRecord(Incomes income)
        {
            return await _incomeDataAccess.DeleteRecordAsync(income);
        }
    }
}
