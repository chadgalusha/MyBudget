using MyBudget.Models;

namespace MyBudget.Helpers
{
	public static class ItemFromList
	{
		public static string GetNameFromIncomeTypeList(int id, List<IncomeTypes> list)
		{
			return list.Where(a => a.IncomeTypeId == id)
				.Select(a => a.IncomeType)
				.First();
		}

		public static string GetNameFromPaymentFrequencyTypeList(int id, List<PaymentFrequencyTypes> list)
		{
			return list.Where(a => a.PaymentFrequencyTypeId == id)
				.Select(a => a.PaymentFrequencyType)
				.First();
		}

		public static string GetNameFromExpenseTypeList(int id, List<ExpenseTypes> list)
		{
			return list.Where(a => a.ExpenseTypeId == id)
				.Select(a => a.ExpenseType)
				.First();
		}
	}
}
