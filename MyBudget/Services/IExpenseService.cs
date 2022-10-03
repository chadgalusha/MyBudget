using MyBudget.Models;

namespace MyBudget.Services
{
	public interface IExpenseService
	{
		Task<Expenses> CreateRecord(Expenses newExpense);
		Task<Expenses> DeleteRecord(Expenses expense);
		Task<Expenses> GetById(int id);
		Task<List<Expenses>> GetList();
		Task<Expenses> UpdateRecord(Expenses expense);
	}
}