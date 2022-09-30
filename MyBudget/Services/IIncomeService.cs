using MyBudget.Models;

namespace MyBudget.Services
{
	public interface IIncomeService
	{
		Task<Incomes> CreateRecord(Incomes newIncome);
		Task<Incomes> DeleteRecord(Incomes income);
		Task<Incomes> GetById(int id);
		Task<List<Incomes>> GetList();
		Task<Incomes> UpdateRecord(Incomes income);
	}
}