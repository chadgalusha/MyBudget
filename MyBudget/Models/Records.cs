using SQLite;

namespace MyBudget.Models
{
	[Table("Records")]
	public class Records
	{
		public int Id { get; set; }
		public decimal IncomeLastYear { get; set; }
		public decimal IncomeThisYear { get; set; }
		public decimal IncomeLastThreeMonths { get; set; }
		public decimal IncomeLastMonth { get; set; }
		public decimal ExpenseLastYear { get; set; }
		public decimal ExpenseThisYear { get; set; }
		public decimal ExpenseLastThreeMonths { get; set; }
		public decimal ExpenseLastMonth { get; set; }
	}
}
