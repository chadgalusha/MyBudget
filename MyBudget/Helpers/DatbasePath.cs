namespace BudgetApplication.Helpers
{
    public class DatbasePath
    {
        public static string GetDbPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "database.sqlite");
        }
    }
}
