using SQLite;

namespace MyBudget.Models
{
	[Table("BankAccountTypes")]
	public class BankAccountTypes
	{
		[PrimaryKey, AutoIncrement]
		public int BankAccountTypeId { get; set; }

		[MaxLength(50), Unique]
		public string BankAccountType { get; set; }

		public static string[] InitialValues()
		{
			string[] initialValues = { "Checking", "Savings", "Money-Market" };
			return initialValues;
		}
	}
}
