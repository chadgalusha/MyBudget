using SQLite;

namespace MyBudget.Models
{
    [Table("ExpenseHistory")]
    public class ExpenseHistory
    {
        [PrimaryKey, AutoIncrement]
        public int ExpenseHistoryId { get; set; }

        [MaxLength(50)]
        public string ExpenseName { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, 999999999.99)]
        public decimal ExpenseAmount { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, 999999999.99)]
        public decimal AmountPaid { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime DueDate { get; set; }

        public int ExpenseCategoryId { get; set; }
    }
}
