using SQLite;

namespace BudgetApplication.Models
{
    [Table("BankAccountTypes")]
    public class BankAccountTypes
    {
        [PrimaryKey, AutoIncrement]
        public int BankAccountTypeId { get; set; }

        [MaxLength(50), Unique]
        public string BankAccountType { get; set; }
    }
}
