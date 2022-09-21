using BudgetApplication.Helpers;
using BudgetApplication.Models;
using SQLite;

namespace MyBudget.Services
{
    public class PaymentFrequencyTypeService
    {
        private string _dbPath = DatbasePath.GetDbPath();
        private SQLiteConnection _connection;

        public PaymentFrequencyTypeService()
        {
            _connection = new SQLiteConnection(_dbPath);
            _connection.CreateTable<PaymentFrequencyTypes>();
        }

        public List<PaymentFrequencyTypes> GetList()
        {
            return _connection.Table<PaymentFrequencyTypes>().ToList();
        }
    }
}
