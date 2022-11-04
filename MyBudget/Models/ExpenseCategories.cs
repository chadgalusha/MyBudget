using SQLite;

namespace MyBudget.Models
{
	public class ExpenseCategories
	{
		[PrimaryKey, AutoIncrement]
		public int ExpenseCategoryId { get; set; }

		[MaxLength(50), Unique]
		public string ExpenseCategoryName { get; set; }

		[MaxLength(450)]
		public string ExpenseCategoryDescription { get; set; }

		public static Dictionary<string, string> InitialValues()
		{
			Dictionary<string, string> initialValues = new() {
				{ "Home & Utility", "Mortgage, rent, electricity, water, gas" },
				{ "Giving", "Charitable giving" },
				{ "Grocery", "Food shopping" },
				{ "Transportation", "Gas and maintenance for vehicles" },
				{ "Health", "Medical, Dental, etc." },
				{ "Finance", "Payments for credit cards, service charges, loan payments" },
				{ "Insurance", "Life insuruance, vehicle insurance, etc." },
				{ "Dine Out", "Restaurants, fast food, coffee" },
				{ "Shopping & Entertainment", "Entertainment spending, gifts, hobbies, frivolous spending" },
				{ "Education", "School material and services" },
				{ "Travel", "Airline luggage fees, hotels, car rentals" },
				{ "Business Expense", "Office supplies, print, business expenses" },
				{ "Misc Cach & Checks", "ATM, money checks, etc." },
				{ "Pet Care", "Health and other spending for pets besides food" },
				{ "No Category", "Spending that defies categorization" },
			};

			return initialValues;
		}
	}
}
