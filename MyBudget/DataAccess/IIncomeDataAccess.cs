using MyBudget.Models;

namespace MyBudget.DataAccess
{
    public interface IIncomeDataAccess : IDataAccess<Incomes>
	{
        bool DoesIncomeNameExist(string incomeName);
        string GetNameOfIncomeById(int id);
    }
}
