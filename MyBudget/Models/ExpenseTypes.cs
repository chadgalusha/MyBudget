using SQLite;

namespace MyBudget.Models
{
    [Table("ExpenseTypes")]
    public class ExpenseTypes
    {
        [PrimaryKey, AutoIncrement]
        public int ExpenseTypeId { get; set; }

        [MaxLength(50), Unique]
        public string ExpenseType { get; set; }
    }
}
