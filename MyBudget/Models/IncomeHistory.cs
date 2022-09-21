using SQLite;

namespace BudgetApplication.Models
{
    [Table("IncomeHistory")]
    public class IncomeHistory
    {
        [PrimaryKey, AutoIncrement]
        public int IncomeHistoryId { get; set; }

        public int IncomeId { get; set; }

        public decimal IncomeAmount { get; set; }

        public DateTime IncomeDate { get; set; }
    }
}
