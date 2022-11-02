namespace MyBudget.DataAccess
{
	public interface ITypeDataAccess<T>
	{
		Task<T> GetRecordByIdAsync(int id);
		Task<List<T>> GetListAsync();
		Task<T> CreateRecord(T newType);
		Task<T> UpdateRecordAsync(T type);
		Task<T> DeleteRecordAsync(T type);
		bool DoesTypeNameExist(string typeName);
		string GetNameOfTypeByID(int typeId);
		bool IsTypeUsedAndCannotBeDeleted(int typeId);
	}
}
