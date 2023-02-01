namespace MyBudget.Helpers
{
	public interface IChartProcessor
	{
		Task<Dictionary<string, decimal>> DonutChartLastMonthExpenses();
	}
}