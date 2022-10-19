using SQLite;

namespace MyBudget.Models
{
    [Table("IncomeHistory")]
    public class IncomeHistory
    {
        [PrimaryKey, AutoIncrement]
        public int IncomeHistoryId { get; set; }

        [MaxLength(50)]
        public string IncomeName { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, 999999999.99)]
        public decimal IncomeAmount { get; set; }

        public DateTime IncomeDate { get; set; }
    }
}
