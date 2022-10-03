using MyBudget.Models;

namespace MyBudget.DataAccess
{
    public interface IIncomeTypeDataAccess : IDataAccess<IncomeTypes>
    {
        bool DoesIncomeTypeNameExist(string incomeTypeName);
        string GetNameOfIncomeTypeById(int id);
        bool IsIncomeTypeUsedByIncome(int incomeTypeId);
    }
}
