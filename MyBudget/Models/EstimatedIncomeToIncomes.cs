using SQLite;

namespace MyBudget.Models
{
    [Table("EstimatedIncomeToIncomes")]
    public class EstimatedIncomeToIncomes
    {
        [PrimaryKey]
        public int EstimatedIncomeId { get; set; }

        [PrimaryKey]
        public int IncomeId { get; set; }
    }
}
