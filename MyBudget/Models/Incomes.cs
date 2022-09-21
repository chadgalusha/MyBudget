using SQLite;

namespace BudgetApplication.Models
{
    [Table("Incomes")]
    public class Incomes
    {
        [PrimaryKey, AutoIncrement]
        public int IncomeId { get; set; }

        [MaxLength(50), Unique]
        public string IncomeName { get; set; }

        public int IncomeTypeId { get; set; }

        public int PaymentFrequencyTypeId { get; set; }
    }
}
