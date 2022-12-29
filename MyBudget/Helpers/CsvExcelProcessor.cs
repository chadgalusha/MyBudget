using ExcelDataReader;
using MyBudget.TempModels;

namespace MyBudget.Helpers
{
	public class CsvExcelProcessor
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

		// Private methods

		private static FilePickerFileType GetFilesAllowed()
		{
			var allowedFiles = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.WinUI, new[] { ".csv", ".xlsx" } },
					{ DevicePlatform.iOS, new[] { ".csv", ".xlsx" } },
					{ DevicePlatform.Android, new[] { ".csv", ".xlsx" } }
				});

			return allowedFiles;
		}

		private (List<TempIncomeHistory>, List<TempExpenseHistory>) ProcessFile(FileResult file)
		{
			(List<TempIncomeHistory>, List<TempExpenseHistory>) tupleResult = new();

			if (file.FileName.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase))
			{
				tupleResult = ExcelProccesor(file);
			}

			if (file.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
			{
				tupleResult = CsvProcessor(file);
			}

			return tupleResult;
		}

		// Process Excel Files
		private (List<TempIncomeHistory>, List<TempExpenseHistory>) ExcelProccesor(FileResult file)
		{
			using (var stream = File.Open(file.FullPath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = ExcelReaderFactory.CreateReader(stream))
				{
					do
					{
						while (reader.Read())
						{
							if (reader.GetDouble(0) < 0)
							{

							}
						}
					} while (reader.NextResult());
				}
			}
		}

		// Process Csv Files
		private (List<TempIncomeHistory>, List<TempExpenseHistory>) CsvProcessor(FileResult file)
		{

		}
	}
}
