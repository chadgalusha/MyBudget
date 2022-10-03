using MyBudget.Models;

namespace MyBudget.DataAccess
{
	public interface IExpenseDataAccess : IDataAccess<Expenses>
	{
		bool DoesExpenseNameExist(string expenseName);
		string GetNameOfExpenseById(int id);
	}
}
