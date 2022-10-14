using MyBudget.DataAccess;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class ExpenseHistoryService : IHistoryService<ExpenseHistory>
    {
        private readonly IHistoryDataAccess<ExpenseHistory> _expenseHistoryDataAccess;

        public ExpenseHistoryService(IHistoryDataAccess<ExpenseHistory> expenseHistoryDataAccess)
        {
            _expenseHistoryDataAccess = expenseHistoryDataAccess;
        }

        public async Task<ExpenseHistory> GetById(int id)
        {
            return await _expenseHistoryDataAccess.GetRecordByIdAsync(id);
        }

        public async Task<List<ExpenseHistory>> GetList()
        {
            return await _expenseHistoryDataAccess.GetListAsync();
        }

        public async Task<ExpenseHistory> CreateRecord(ExpenseHistory newExpenseHistory)
        {
            return await _expenseHistoryDataAccess.CreateRecordAsync(newExpenseHistory);
        }

        public async Task<ExpenseHistory> UpdatRecord(ExpenseHistory expenseHistory)
        {
            return await _expenseHistoryDataAccess.UpdateRecordAsync(expenseHistory);
        }

        public async Task<ExpenseHistory> DeleteRecord(ExpenseHistory expenseHistory)
        {
            return await _expenseHistoryDataAccess.DeleteRecordAsync(expenseHistory);
        }

        public decimal GetAmountForLastMonth()
        {
            DateTime dateTime = DateTime.Now;
            int year = GetYearFromDateTime(dateTime);
            int month = GetPreviousMonth(dateTime);

            decimal[] expenseArray = _expenseHistoryDataAccess.GetHistoryArrayForMonth(year, month);
            return AddUpTotal(expenseArray);
        }

        public decimal GetAmountForThisMonth()
        {
            DateTime dateTime = DateTime.Now;
            decimal[] expenseArray = _expenseHistoryDataAccess.GetHistoryArrayForMonth(dateTime.Year, dateTime.Month);

            return AddUpTotal(expenseArray);
        }

        public decimal GetAmountForLastYear()
        {
            int lastYear = DateTime.Now.Year - 1;
            decimal[] expenseArray = _expenseHistoryDataAccess.GetHistoryArrayForYear(lastYear);

            return AddUpTotal(expenseArray);
        }

        public decimal GetAmountForThisYear()
        {
            int currentYear = DateTime.Now.Year;
            decimal[] expenseArray = _expenseHistoryDataAccess.GetHistoryArrayForYear(currentYear);

            return AddUpTotal(expenseArray);
        }

        // PRIVATE METHODS

        private static decimal AddUpTotal(decimal[] amountArray)
        {
            decimal total = 0;

            foreach (var amount in amountArray)
            {
                total += amount;
            }

            return total;
        }

        private static int GetYearFromDateTime(DateTime dateTime)
        {
            if (GetPreviousMonth(dateTime) == 12)
            {
                return dateTime.Year - 1;
            }
            else
            {
                return dateTime.Year;
            }
        }

        private static int GetPreviousMonth(DateTime dateTime)
        {
            int previousMonth = dateTime.Month - 1;
            return previousMonth == 0 ? 12 : previousMonth;
        }
    }
}
