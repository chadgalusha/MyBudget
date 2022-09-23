using MyBudget.Models;

namespace MyBudget.DataAccess
{
    public interface IDataAccess<T>
    {
        Task<T> CreateRecord(T newType);
        Task<T> DeleteRecordAsync(T type);
        Task<List<T>> GetListAsync();
        Task<T> GetRecordByIdAsync(int id);
        Task<T> UpdateRecordAsync(T type);
    }
}