namespace MyBudget.Helpers
{
	public static class ItemFromList
	{
		public static string GetNameFromTypeList<T>(int id, List<T> list)
		{
			string name = "";

			foreach (var obj in list)
			{
				var objId = GetTypeIdFromObject(obj);

				if (objId == id)
				{
					name = GetNameFromObject(obj);
				}
			}

			return name;
		}

		// private methods

		private static int GetTypeIdFromObject<T>(T obj)
		{
			var id = obj.GetType()
                    .GetProperties()
                    .First(a => a.Name.Contains("TypeId"))
                    .GetValue(obj);

			return Convert.ToInt32(id);
        }

		private static string GetNameFromObject<T>(T obj)
		{
			var name = obj.GetType()
						.GetProperties()
						.First(a => !a.Name.Contains("Id") && a.Name.Contains("Type"))
						.GetValue(obj);

			return name.ToString();
        }
	}
}
