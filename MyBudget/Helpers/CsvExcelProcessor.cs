namespace MyBudget.Helpers
{
	public class CsvExcelProcessor
	{
		public async Task<FileResult> Csv_Xslx_Selector()
		{
			var result = await FilePicker.PickAsync(new PickOptions
			{
				PickerTitle = "Select .xlsx or .csv file",
				FileTypes = FilesAllowed()
			});

			return result;
		}

		private static FilePickerFileType FilesAllowed()
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
	}
}
