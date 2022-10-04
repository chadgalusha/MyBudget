using SQLite;

namespace MyBudget.Models
{
    [Table("IncomeTypes")]
    public class IncomeTypes
    {
        [PrimaryKey, AutoIncrement]
        public int IncomeTypeId { get; set; }

        [MaxLength(50), Unique]
        public string IncomeType { get; set; }

        public static string[] InitialValues()
        {
            string[] initialValues = { "Primary", "Secondary", "Occasional", "One-Time" };
            return initialValues;
        }
    }
}
