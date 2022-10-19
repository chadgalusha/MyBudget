using MyBudget.Models;

namespace MyBudget.Services
{
    public interface IHistoryService<T>
    {
        Task<T> GetById(int id);
        Task<List<T>> GetList();
        Task<T> CreateRecord(T newHistory);
        Task<T> UpdateRecord(T history);
        Task<T> DeleteRecord(T history);
        decimal GetAmountForLastMonth();
        decimal GetAmountForLastYear();
        decimal GetAmountForThisMonth();
        decimal GetAmountForThisYear();
    }
}