using SQLite;

namespace MyBudget.Models
{
	[Table("Expenses")]
	public class Expenses
	{
		[PrimaryKey, AutoIncrement]
		public int ExpensesId { get; set; }

		[MaxLength(50), Unique]
		public string ExpensesName { get; set; }

		public int ExpenseTypeId { get; set; }

		public int PaymentFrequencyTypeId { get; set; }

		[System.ComponentModel.DataAnnotations.Range(0, 999999999.99)]
		public decimal ExpenseAmount { get; set; }

		public DateTime InitialExpenseDate { get; set; }
	}
}
