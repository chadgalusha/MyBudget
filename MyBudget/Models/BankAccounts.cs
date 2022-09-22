using SQLite;

namespace MyBudget.Models
{
    [Table("BankAccounts")]
    public class BankAccounts
    {
        [PrimaryKey, AutoIncrement]
        public int BankAccountId { get; set; }

        [MaxLength(50), Unique]
        public string BankAccountName { get; set; }

        public int BankAccountTypeId { get; set; }

        public decimal Balance { get; set; }
    }
}
