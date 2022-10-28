using MyBudget.DataAccess;
using MyBudget.Helpers;
using MyBudget.Models;

namespace MyBudget.Services
{
    public class IncomeHistoryService : IHistoryService<IncomeHistory>
    {
        private readonly IHistoryDataAccess<IncomeHistory> _incomeHistoryDataAccess;

        public IncomeHistoryService(IHistoryDataAccess<IncomeHistory> incomeHistoryDataAccess)
        {
            _incomeHistoryDataAccess = incomeHistoryDataAccess;
        }

        public Task<IncomeHistory> GetById(int id)
        {
            return _incomeHistoryDataAccess.GetRecordByIdAsync(id);
        }

        public Task<List<IncomeHistory>> GetList()
        {
            return _incomeHistoryDataAccess.GetListAsync();
        }

        public async Task<IncomeHistory> CreateRecord(IncomeHistory newIncomeHistory)
        {
            try
            {
                return await _incomeHistoryDataAccess.CreateRecordAsync(newIncomeHistory);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorCreating(newIncomeHistory, e);
                return new IncomeHistory() { IncomeHistoryId = 0 };
            }
        }

        public async Task<IncomeHistory> UpdateRecord(IncomeHistory incomeHistory)
        {
            try
            {
                return await _incomeHistoryDataAccess.UpdateRecordAsync(incomeHistory);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorUpdating(incomeHistory, e);
                return new IncomeHistory() { IncomeHistoryId = 0 };
            }
        }

        public async Task<IncomeHistory> DeleteRecord(IncomeHistory incomeHistory)
        {
            try
            {
                return await _incomeHistoryDataAccess.DeleteRecordAsync(incomeHistory);
            }
            catch (Exception e)
            {
                MyBudgetLogger.ErrorDeleting(incomeHistory, e);
                return new IncomeHistory() { IncomeHistoryId = 0 };
            }
        }

        public decimal GetAmountForLastMonth()
        {
            DateTime dateTime = DateTime.Now;
            int year = GetYearFromDateTime(dateTime);
            int month = GetPreviousMonth(dateTime);

            decimal[] incomeArray = _incomeHistoryDataAccess.GetHistoryArrayForMonth(year, month);
            return AddUpTotal(incomeArray);
        }
        public decimal GetAmountForThisMonth()
        {
            DateTime dateTime = DateTime.Now;
            decimal[] incomeArray = _incomeHistoryDataAccess.GetHistoryArrayForMonth(dateTime.Year, dateTime.Month);

            return AddUpTotal(incomeArray);
        }

        public decimal GetAmountForLastYear()
        {
            int lastYear = DateTime.Now.Year - 1;
            decimal[] incomeArray = _incomeHistoryDataAccess.GetHistoryArrayForYear(lastYear);

            return AddUpTotal(incomeArray);
        }

        public decimal GetAmountForThisYear()
        {
            int currentYear = DateTime.Now.Year;
            decimal[] incomeArray = _incomeHistoryDataAccess.GetHistoryArrayForYear(currentYear);

            return AddUpTotal(incomeArray);
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
