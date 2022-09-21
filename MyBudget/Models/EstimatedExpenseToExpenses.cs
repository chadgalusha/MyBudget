using SQLite;

namespace BudgetApplication.Models
{
    [Table("EstimatedExpenseToExpenses")]
    public class EstimatedExpenseToExpenses
    {
        [PrimaryKey]
        public int EstimatedExpenseId { get; set; }

        [PrimaryKey]
        public int ExpenseId { get; set; }
    }
}
