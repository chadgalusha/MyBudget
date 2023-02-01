using MyBudget.Models;
using MyBudget.Services;

namespace MyBudget.Helpers
{
	public class ChartProcessor : IChartProcessor
	{
		private readonly IHistoryService<ExpenseHistory> _expenseHistoryService;
		private readonly IService<ExpenseCategories> _expenseCategoryService;

		public ChartProcessor(IHistoryService<ExpenseHistory> expenseHistoryService, IService<ExpenseCategories> expenseCategoryService)
		{
			_expenseHistoryService = expenseHistoryService;
			_expenseCategoryService = expenseCategoryService;
		}

		public async Task<Dictionary<string, decimal>> DonutChartLastMonthExpenses()
		{
			List<ExpenseHistory> expenseHistoryList = GetLastMonthExpenseHistory();
			List<ExpenseCategories> expenseCategoryList = await GetListExpenseCategoriesAsync();

			return GetDictionaryData(expenseHistoryList, expenseCategoryList);
		}

		// Private Methods

		private Dictionary<string, decimal> GetDictionaryData(List<ExpenseHistory> expenseHistoryList, List<ExpenseCategories> expenseCategoryList)
		{
			Dictionary<string, decimal> donutChartData = new();
			string expenseName;
			decimal total;
			int id;

			foreach (var category in expenseCategoryList)
			{
				id = category.ExpenseCategoryId;

				expenseName = GetExpenseCategoryName(expenseCategoryList, id);
				total = GetTotalForExpenseCategory(expenseHistoryList, id);

				if (total > 0.00M)
				{
					donutChartData.Add(expenseName, total);
				}
			}

			return donutChartData;
		}

		private List<ExpenseHistory> GetLastMonthExpenseHistory()
		{
			DateTime lastMonthDateTime = DateTime.Now.AddMonths(-1);
			return _expenseHistoryService.GetListByYearMonth(lastMonthDateTime.Year, lastMonthDateTime.Month);
		}

		private async Task<List<ExpenseCategories>> GetListExpenseCategoriesAsync()
		{
			return await _expenseCategoryService.GetList();
		}

		private decimal GetTotalForExpenseCategory(List<ExpenseHistory> expenseHistoryList, int id)
		{
			return expenseHistoryList
				.Where(e => e.ExpenseCategoryId == id)
				.Select(e => e.AmountPaid)
				.Sum();
		}

		private string GetExpenseCategoryName(List<ExpenseCategories> expenseCategoryList, int id)
		{
			return expenseCategoryList
				.Where(e => e.ExpenseCategoryId == id)
				.Select(e => e.ExpenseCategoryName)
				.FirstOrDefault("Unknown");
		}
	}
}
