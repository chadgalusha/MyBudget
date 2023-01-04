using ExcelDataReader;
using MyBudget.TempModels;
using System.Text;

namespace MyBudget.Helpers
{
	public class CsvExcelProcessor : ICsvExcelProcessor
	{
		private readonly string error = "error";
		private readonly FilePickerFileType allowedFileTypes;

		public CsvExcelProcessor()
		{
			allowedFileTypes = GetFilesAllowed();
		}

		public async Task<FileResult> Csv_Xslx_Selector()
		{
			try
			{
				var result = await FilePicker.PickAsync(new PickOptions
				{
					PickerTitle = "Select .xlsx or .csv file",
					FileTypes = allowedFileTypes
				});

				return result ??= new(error);
			}
			catch (Exception e)
			{
				MyBudgetLogger.ErrorLogMessage(e);
				return new(error);
			}
		}

		// Process .xslx or .csv file. As file can have incomes and expenses, send back separate lists in tuple(incomes, expenses)
		public (List<TempIncomeHistory>, List<TempExpenseHistory>) ProcessFile(FileResult file)
		{
			(List<TempIncomeHistory>, List<TempExpenseHistory>) tupleResult = new(GetNewTempIncomeList(), GetNewTempExpenseList());

			if (file.FileName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase))
			{
				tupleResult = ExcelProccesor(file, tupleResult);
			}

			if (file.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
			{
				tupleResult = CsvProcessor(file, tupleResult);
			}

			return tupleResult;
		}

		#region Private Methods

		private static FilePickerFileType GetFilesAllowed()
		{
			var allowedFiles = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.WinUI,   new[] { ".csv", ".xlsx" } },
					{ DevicePlatform.iOS,     new[] { ".csv", ".xlsx" } },
					{ DevicePlatform.Android, new[] { ".csv", ".xlsx" } }
				});

			return allowedFiles;
		}

		// Process Excel (.xslx) Files
		private (List<TempIncomeHistory>, List<TempExpenseHistory>) ExcelProccesor(FileResult file,
			(List<TempIncomeHistory>, List<TempExpenseHistory>) tupleResult)
		{
			using (var stream = File.Open(file.FullPath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					do
					{
						while (reader.Read())
						{
							if (reader.GetDouble(2) > 0)
							{
								tupleResult.Item1.Add(new TempIncomeHistory
								{
									IncomeHistoryId = NextId(tupleResult.Item1),
									IncomeName      = NameUpTo40Chars(reader.GetString(1)),
									IncomeAmount    = Convert.ToDecimal(reader.GetDouble(2)),
									IncomeDate      = reader.GetDateTime(0)
								});
							}
							if (reader.GetDouble(2) < 0)
							{
								tupleResult.Item2.Add(new TempExpenseHistory
								{
									ExpenseHistoryId  = NextId(tupleResult.Item2),
									ExpenseName		  = NameUpTo40Chars(reader.GetString(1)),
									AmountPaid		  = Math.Abs(Convert.ToDecimal(reader.GetDouble(2))),
									ExpenseDate		  = reader.GetDateTime(0),
									ExpenseCategoryId = 1
								});
							}

						}
					} while (reader.NextResult());
				}
			}

			return tupleResult;
		}

		// Process Csv Files
		private (List<TempIncomeHistory>, List<TempExpenseHistory>) CsvProcessor(FileResult file,
			(List<TempIncomeHistory>, List<TempExpenseHistory>) tupleResult)
		{
			using (var stream = File.Open(file.FullPath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateCsvReader(stream, CsvConfiguration()))
				{
					do
					{
						while (reader.Read())
						{
							if (Convert.ToDouble(reader.GetString(2)) > 0)
							{
								tupleResult.Item1.Add(new TempIncomeHistory
								{
									IncomeHistoryId = NextId(tupleResult.Item1),
									IncomeName		= NameUpTo40Chars(reader.GetString(1)),
									IncomeAmount    = Convert.ToDecimal(reader.GetString(2)),
									IncomeDate		= Convert.ToDateTime(reader.GetString(0))
								});
							}
							if (Convert.ToDouble(reader.GetString(2)) < 0)
							{
								tupleResult.Item2.Add(new TempExpenseHistory
								{
									ExpenseHistoryId  = NextId(tupleResult.Item2),
									ExpenseName		  = NameUpTo40Chars(reader.GetString(1)),
									AmountPaid		  = Math.Abs(Convert.ToDecimal(reader.GetString(2))),
									ExpenseDate		  = Convert.ToDateTime(reader.GetString(0)),
									ExpenseCategoryId = 1
								});
							}
						}
					} while (reader.NextResult());
				}
			}

			return tupleResult;
		}

		private static ExcelReaderConfiguration CsvConfiguration()
		{
			return new ExcelReaderConfiguration
			{
				FallbackEncoding = Encoding.GetEncoding(1252),
				AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
				LeaveOpen = false,
				AnalyzeInitialCsvRows = 0
			};
		}

		private static string NameUpTo40Chars(string name)
		{
			return name.Length > 40 ? name[..40] : name;
		}

		private static int NextId<T>(List<T> list)
		{
			return list.Count + 1;
		}

		private static List<TempIncomeHistory> GetNewTempIncomeList() => new();

		private static List<TempExpenseHistory> GetNewTempExpenseList() => new();

		#endregion
	}
}
