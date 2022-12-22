namespace MyBudget.TempModels
{
	public class TempExpenseHistory
	{
		public int ExpenseHistoryId { get; set; }
		public string ExpenseName { get; set; }
		public decimal AmountPaid { get; set; }
		public DateTime? ExpenseDate { get; set; }
		public int ExpenseCategoryId { get; set; }
	}

	public class TempIncomeHistory
	{
		public int IncomeHistoryId { get; set; }
		public string IncomeName { get; set; }
		public decimal IncomeAmount { get; set; }
		public DateTime? IncomeDate { get; set; }
	}
}
