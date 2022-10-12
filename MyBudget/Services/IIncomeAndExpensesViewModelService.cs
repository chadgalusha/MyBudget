namespace MyBudget.Services
{
	public interface IIncomeAndExpensesViewModelService
	{
		decimal LastMonthExpenses();
		decimal LastMonthIncome();
		decimal LastYearExpenses();
		decimal LastYearIncome();
		decimal ThisMonthExpenses();
		decimal ThisMonthIncome();
		decimal ThisYearExpenses();
		decimal ThisYearIncome();
	}
}