using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBudget.Data
{
	public class DataItem
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("value")]
		public decimal Value { get; set; }

		[JsonPropertyName("group")]
		public string Group { get; set; }
	}
}
