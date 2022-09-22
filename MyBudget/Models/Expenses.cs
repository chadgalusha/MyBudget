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
    }
}
