using MyBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.DataAccess
{
    public interface IExpenseTypeDataAccess : IDataAccess<ExpenseTypes>
    {
        bool DoesExpenseTypeNameExist(string expenseTypeName);
        string GetNameOfExpenseTypeById(int id);
        bool IsExpenseTypeUsedByExpense(int expenseTypeId);
    }
}
