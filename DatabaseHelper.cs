using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ManipalDMS
{
    internal static class DatabaseHelper
    {
        private static string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["PressDMS"];
                if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                    throw new InvalidOperationException("The PressDMS connection string is missing from App.config.");

                return settings.ConnectionString;
            }
        }

        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                AddParameters(command, parameters);
                DataTable results = new DataTable();
                adapter.Fill(results);
                return results;
            }
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                AddParameters(command, parameters);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public static void LogActivity(string username, string action, string tableAffected, string details)
        {
            const string query = "INSERT INTO AuditLog (Username, Action, TableAffected, Details) " +
                                 "VALUES (@Username, @Action, @TableAffected, @Details)";
            ExecuteNonQuery(query,
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username ?? string.Empty },
                new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = action },
                new SqlParameter("@TableAffected", SqlDbType.NVarChar, 50) { Value = tableAffected },
                new SqlParameter("@Details", SqlDbType.NVarChar, 255) { Value = details ?? string.Empty });
        }

        private static void AddParameters(SqlCommand command, SqlParameter[] parameters)
        {
            if (parameters == null) return;
            foreach (SqlParameter parameter in parameters)
                command.Parameters.Add(parameter);
        }
    }
}
