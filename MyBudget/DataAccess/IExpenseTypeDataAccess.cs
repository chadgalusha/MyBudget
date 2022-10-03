using MyBudget.Models;

namespace MyBudget.DataAccess
{
    public interface IExpenseTypeDataAccess : IDataAccess<ExpenseTypes>
    {
        bool DoesExpenseTypeNameExist(string expenseTypeName);
        string GetNameOfExpenseTypeById(int id);
        bool IsExpenseTypeUsedByExpense(int expenseTypeId);
    }
}
