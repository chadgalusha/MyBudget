namespace MyBudget.Helpers
{
	public class CalendarProcessor : ICalendarProcessor
	{
		public int IntForFirstDayOfMonth(int year, int month)
		{
			DateTime dateTime = new(year, month, 1);
			DayEnum dayOfWeek = (DayEnum)dateTime.DayOfWeek;
			return (int)dayOfWeek;
		}

		public int NumberDaysInMonth(int year, int month)
		{
			return DateTime.DaysInMonth(year, month);
		}

		public string GetMonthString(int month)
		{
			var monthEnum = (MonthsEnum)month;
			return monthEnum.ToString();
		}
	}
}
