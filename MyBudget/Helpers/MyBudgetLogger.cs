using Serilog;
using System.Text.Json;

namespace MyBudget.Helpers
{
    public static class MyBudgetLogger
    {
        // ERROR MESSAGES

        public static void ErrorCreating<T>(T type, Exception e)
        {
            Log.Error($"Error creating new {type.GetType()}: {e.Message} \n\tType data: {GetJsonSerializedType(type)}");
        }

        public static void ErrorUpdating<T>(T type, Exception e)
        {
            Log.Error($"Error updating {type.GetType()}: {e.Message} \n\tType data: {GetJsonSerializedType(type)}");
        }

        public static void ErrorDeleting<T>(T type, Exception e)
        {
            Log.Error($"Error deleting {type.GetType()}: {e.Message} \n\tType data: {GetJsonSerializedType(type)}");
        }

        // CRUD messages

        public static void CreatedLogMessage<T>(T type)
        {
            Log.Information($"{type.GetType()} created: {GetJsonSerializedType(type)}");
        }

        public static void UpdatedLogMessage<T>(T type)
        {
            Log.Information($"{type.GetType()} updated: {GetJsonSerializedType(type)}");
        }

        public static void DeletedLogMessage<T>(T type)
        {
            Log.Information($"{type.GetType()} deleted: {GetJsonSerializedType(type)}");
        }

        // PRIVATE METHODS

        private static string GetJsonSerializedType<T>(T type)
        {
            return JsonSerializer.Serialize(type);
        }
    }
}
