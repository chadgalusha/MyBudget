using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.Data
{
	public class SampleData
	{
		public static List<string> MyBarText = new List<string>() { "test1", "test2" };

		public static List<DataItem> MyBar = new List<DataItem>()
		{
			new DataItem() { Name = "test1", Value = 415.07M },
			new DataItem() { Name = "test2", Value = 378.52M }
		};
	}
}
