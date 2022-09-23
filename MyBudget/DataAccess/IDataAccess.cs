using MyBudget.Models;

namespace MyBudget.DataAccess
{
    public interface IDataAccess<T>
    {
        Task<T> GetRecordByIdAsync(int id);
        Task<List<T>> GetListAsync();
        Task<T> CreateRecord(T newType);
        Task<T> UpdateRecordAsync(T type);
        Task<T> DeleteRecordAsync(T type);
    }
}