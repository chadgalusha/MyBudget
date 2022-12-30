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

		// Private methods

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

		// Process Excel Files
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
									IncomeName = reader.GetString(1)[..30],
									IncomeAmount = Convert.ToDecimal(reader.GetDouble(2)),
									IncomeDate = reader.GetDateTime(0)
								});
							}
							if (reader.GetDouble(2) < 0)
							{
								tupleResult.Item2.Add(new TempExpenseHistory
								{
									ExpenseName = reader.GetString(1)[..30],
									AmountPaid = Convert.ToDecimal(reader.GetDouble(2)),
									ExpenseDate = reader.GetDateTime(0)
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
									IncomeName = reader.GetString(1)[..30],
									IncomeAmount = Convert.ToDecimal(reader.GetString(2)),
									IncomeDate = Convert.ToDateTime(reader.GetString(0))
								});
							}
							if (Convert.ToDouble(reader.GetString(2)) < 0)
							{
								tupleResult.Item2.Add(new TempExpenseHistory
								{
									ExpenseName = reader.GetString(1)[..30],
									AmountPaid = Convert.ToDecimal(reader.GetString(2)),
									ExpenseDate = Convert.ToDateTime(reader.GetString(0))
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

		private static List<TempIncomeHistory> GetNewTempIncomeList() => new List<TempIncomeHistory>();

		private static List<TempExpenseHistory> GetNewTempExpenseList() => new List<TempExpenseHistory>();
	}
}
