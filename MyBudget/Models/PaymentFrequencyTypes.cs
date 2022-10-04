using SQLite;

namespace MyBudget.Models
{
    [Table("PaymentFrequencyTypes")]
    public class PaymentFrequencyTypes
    {
        [PrimaryKey, AutoIncrement]
        public int PaymentFrequencyTypeId { get; set; }

        [MaxLength(50), Unique]
        public string PaymentFrequencyType { get; set; }

        public static string[] InitialValues() 
        {
            string[] initialValues = {"Yearly", "Bi-Yearly", "Quarterly", "Monthly", "Bi-Monthly",
                                      "Bi-Weekly", "Weekly", "Occasional", "Single"};
            return initialValues;
        }
    }
}
