namespace MyBudget.Services
{
	public interface ITypeService<T>
	{
		Task<T> GetById(int id);
		Task<List<T>> GetList();
		Task<T> CreateRecord(T newType);
		Task<T> UpdateRecord(T type);
		Task<T> DeleteRecord(T type);
	}
}
