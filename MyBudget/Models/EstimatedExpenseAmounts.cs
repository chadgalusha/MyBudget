using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetApplication.Models
{
    [Table("EstimatedExpensAmounts")]
    public class EstimatedExpenseAmounts
    {
        [PrimaryKey, AutoIncrement]
        public int EstimatedExpenseId { get; set; }

        public string EstimatedExpenseName { get; set; }

        public decimal EstimatedExpenseAmount { get; set; }

        public DateTime EstimatedExpenseDate { get; set; }

        public bool IsFulfilled { get; set; }
    }
}
