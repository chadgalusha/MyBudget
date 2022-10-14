using MyBudget.Models;

namespace MyBudget.Services
{
    public class IncomeAndExpensesViewModelService : IIncomeAndExpensesViewModelService
    {
        private readonly IHistoryService<IncomeHistory> _incomeHistoryService;
        private readonly IHistoryService<ExpenseHistory> _expenseHistoryService;

        public IncomeAndExpensesViewModelService(IHistoryService<IncomeHistory> incomeHistoryService,
                                                 IHistoryService<ExpenseHistory> expenseHistoryService)
        {
            _incomeHistoryService = incomeHistoryService;
            _expenseHistoryService = expenseHistoryService;
        }

        public decimal LastMonthIncome()
        {
            return _incomeHistoryService.GetAmountForLastMonth();
        }

        public decimal ThisMonthIncome()
        {
            return _incomeHistoryService.GetAmountForThisMonth();
        }

        public decimal LastYearIncome()
        {
            return _incomeHistoryService.GetAmountForLastYear();
        }

        public decimal ThisYearIncome()
        {
            return _incomeHistoryService.GetAmountForThisYear();
        }

        public decimal LastMonthExpenses()
        {
            return _expenseHistoryService.GetAmountForLastMonth();
        }

        public decimal ThisMonthExpenses()
        {
            return _expenseHistoryService.GetAmountForThisMonth();
        }

        public decimal LastYearExpenses()
        {
            return _expenseHistoryService.GetAmountForLastYear();
        }

        public decimal ThisYearExpenses()
        {
            return _expenseHistoryService.GetAmountForThisYear();
        }
    }
}
