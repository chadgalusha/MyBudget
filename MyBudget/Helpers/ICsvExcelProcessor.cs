using MyBudget.TempModels;

namespace MyBudget.Helpers
{
	public interface ICsvExcelProcessor
	{
		Task<FileResult> Csv_Xslx_Selector();
		(List<TempIncomeHistory>, List<TempExpenseHistory>) ProcessFile(FileResult file);
	}
}