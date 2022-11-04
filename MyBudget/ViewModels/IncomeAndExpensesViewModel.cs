using System.Text.Json.Serialization;

namespace MyBudget.ViewModels
{
	public class IncomeAndExpensesViewModel
	{
		[JsonPropertyName("Income")]
		public string Income { get; set; }

		[JsonPropertyName("Income Amount")]
		public decimal IncomeAmount { get; set; }

		[JsonPropertyName("Expenses")]
		public string Expenses { get; set; }

		[JsonPropertyName("Expenses Amount")]
		public decimal ExpensesAmount { get; set; }
	}
}
