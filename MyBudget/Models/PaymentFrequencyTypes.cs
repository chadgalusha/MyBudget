using SQLite;

namespace BudgetApplication.Models
{
    [Table("PaymentFrequencyTypes")]
    public class PaymentFrequencyTypes
    {
        [PrimaryKey, AutoIncrement]
        public int PaymentFrequencyTypeId { get; set; }

        [MaxLength(50), Unique]
        public string PaymentFrequencyType { get; set; }
    }
}
