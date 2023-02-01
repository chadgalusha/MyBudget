namespace MyBudget.Helpers
{
	public interface ICalendarProcessor
	{
        int IntForFirstDayOfMonth(int year, int month);
        int NumberDaysInMonth(int year, int month);
        string GetMonthString(int month);
		DateTime PreviousMonth(DateTime currentDateTime);
		DateTime NextMonth(DateTime currentDateTime);
	}
}