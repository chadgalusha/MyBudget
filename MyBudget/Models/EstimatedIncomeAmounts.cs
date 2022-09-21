using SQLite;

namespace BudgetApplication.Models
{
    [Table("EstimatedIncomeAmounts")]
    public class EstimatedIncomeAmounts
    {
        [PrimaryKey, AutoIncrement]
        public int EstimatedIncomeId { get; set; }

        [MaxLength(50)]
        public string EstimatedIncomeName { get; set; }

        public decimal EstimatedIncomeAmount { get; set; }

        public DateTime EstimatedIncomeDate { get; set; }

        public bool IsFulfilled { get; set; }
    }
}
