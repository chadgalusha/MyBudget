using MyBudget.Models;

namespace MyBudget.Services
{
    public interface ITypeService<T>
    {
        Task<T> CreateRecord(T newType);
        Task<T> DeleteRecord(T type);
        Task<T> GetById(int id);
        Task<List<T>> GetList();
        Task<T> UpdateRecord(T type);
    }
}