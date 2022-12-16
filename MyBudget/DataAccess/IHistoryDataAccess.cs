using MyBudget.Models;

namespace MyBudget.DataAccess
{
	public interface IHistoryDataAccess<T>
	{
		Task<T> GetRecordByIdAsync(int id);
		Task<List<T>> GetListAsync();
		Task<T> CreateRecordAsync(T newType);
		Task<T> UpdateRecordAsync(T type);
		Task<T> DeleteRecordAsync(T type);
		decimal[] GetHistoryArrayForMonth(int year, int month);
		decimal[] GetHistoryArrayForYear(int year);
		List<T> GetListByYearMonth(int year, int month);
    }
}
