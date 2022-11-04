using SQLite;

namespace MyBudget.Models
{
	[Table("Incomes")]
	public class Incomes
	{
		[PrimaryKey, AutoIncrement]
		public int IncomeId { get; set; }

		[MaxLength(50), Unique]
		public string IncomeName { get; set; }

		public int IncomeTypeId { get; set; }

		public int PaymentFrequencyTypeId { get; set; }

		[System.ComponentModel.DataAnnotations.Range(0, 999999999.99)]
		public decimal IncomeAmount { get; set; }

		public DateTime InitialIncomeDate { get; set; }
	}
}
