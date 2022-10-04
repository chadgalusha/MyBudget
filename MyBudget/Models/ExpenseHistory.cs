using SQLite;

namespace MyBudget.Models
{
    [Table("ExpenseHistory")]
    public class ExpenseHistory
    {
        [PrimaryKey, AutoIncrement]
        public int ExpenseHistoryId { get; set; }

        public int ExpenseId { get; set; }

        public decimal ExpenseAmount { get; set; }

        public DateTime ExpenseDate { get; set; }
    }
}
