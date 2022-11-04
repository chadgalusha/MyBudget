namespace MyBudget.Services
{
	public interface IService<T>
	{
		Task<T> GetById(int id);
		Task<List<T>> GetList();
		Task<T> CreateRecord(T newRecord);
		Task<T> UpdateRecord(T record);
		Task<T> DeleteRecord(T record);
	}
}
