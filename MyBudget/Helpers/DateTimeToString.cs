namespace MyBudget.Helpers
{
	public static class DateTimeToString
	{
		public static string ChangeDateTimeToString(DateTime dateTime)
		{
			DateAsString dateAsString = new()
			{
				Year = dateTime.Year.ToString(),
				Month = dateTime.Month.ToString(),
				Day = dateTime.Day.ToString()
			};

			return dateAsString.DateOutput();
		}

		internal class DateAsString
		{
			public string Year {get;set;}
            public string Month {get;set;}
            public string Day {get;set;}

            public string DateOutput()
			{
				return $"{Year}-{Month}-{Day}";
			}
		}
	}
}
