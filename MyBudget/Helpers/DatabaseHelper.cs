﻿using Serilog;
using SQLite;
using System.Reflection;

namespace MyBudget.Helpers
{
    public class DatabaseHelper
    {
        public static string GetDbPath()
        {
            //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "database.sqlite");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test_database.sqlite");
        }

        // When application starts check if db tables exist. Loop through types from Models folder,
        // add any tables to db that are not present
        public static void CheckForDbTables()
        {
            using (SQLiteConnection connection = new(GetDbPath()))
            {
                const string query = "SELECT tbl_name FROM sqlite_master WHERE type = \"table\"";
                var dbTables = connection.Query<sqlite_master>(query);

                Type[] typeArray = GetTypesInModels(Assembly.GetExecutingAssembly(), "MyBudget.Models");

                var newTypeArray = TablesToCreate(dbTables, typeArray);

                if (newTypeArray.Length > 0)
                {
                    connection.CreateTables(createFlags: CreateFlags.None, newTypeArray);
                    LogTablesCreated(newTypeArray);
                }
            }
        }

        // PRIVATE METHODS

        private static Type[] GetTypesInModels(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes()
                .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                .ToArray();
        }

        private static Type[] TablesToCreate(List<sqlite_master> dbTables, Type[] typeArray)
        {
            List<Type> typeList = new();

            foreach (var type in typeArray)
            {
                bool tableExists = dbTables.Any(d => d.tbl_name == type.Name);
                if (tableExists == false)
                {
                    typeList.Add(type);
                }
            }

            return typeList.ToArray();
        }

        private static void LogTablesCreated(Type[] typeList)
        {
            foreach (var type in typeList)
            {
                Log.Information($"Table created: {type.Name}");
            }
        }
    }

    // Separate Model class to reference master files in sqlite db.
    // Will not be accessed when referencing Models folder to ensure db tables are created
    public class sqlite_master
    {
        public string type { get; set; }
        public string name { get; set; }
        public string tbl_name { get; set; }
        public int rootpage { get; set; }
        public string sql { get; set; }
    }
}
