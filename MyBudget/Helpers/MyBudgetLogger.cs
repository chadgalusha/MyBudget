using Serilog;
using System.Text.Json;

namespace MyBudget.Helpers
{
    public static class MyBudgetLogger
    {
        // ERROR MESSAGES

        public static void LogErrorCreating<T>(T type, Exception e)
        {
            var stringType = GetSerializedType(type);
            Log.Error($"Error creating new {type.GetType()}: {e.Message} \n\tType data: {stringType}");
        }

        public static void LogErrorUpdating<T>(T type, Exception e)
        {
            var stringType = GetSerializedType(type);
            Log.Error($"Error updating {type.GetType()}: {e.Message} \n\tType data: {stringType}");
        }

        public static void LoggErrorDeleting<T>(T type, Exception e)
        {
            var stringType = GetSerializedType(type);
            Log.Error($"Error deleting {type.GetType()}: {e.Message} \n\tType data: {stringType}");
        }

        //

        public static void CreatedMessage<T>(T type)
        {
            var stringType = GetSerializedType(type);
            Log.Information($"{type.GetType()} created: {stringType}");
        }

        public static void UpdatedMessage<T>(T type)
        {
            var stringType = GetSerializedType(type);
            Log.Information($"{type.GetType()} updated: {stringType}");
        }

        public static void DeletedMessage<T>(T type)
        {
            var stringType = GetSerializedType(type);
            Log.Information($"{type.GetType()} deleted: {stringType}");
        }

        // PRIVATE METHODS

        private static string GetSerializedType<T>(T type)
        {
            return JsonSerializer.Serialize(type);
        }
    }
}
