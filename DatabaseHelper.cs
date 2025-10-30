using System;
using System.Data;
using System.Data.SqlClient;

namespace ManipalDMS
{
    internal class DatabaseHelper
    {
        private static string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=PressDMS;User ID=sa;Password=shreeyakamatH7";

        public static SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }

        public static DataTable ExecuteQuery(string query)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static void ExecuteNonQuery(string query)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void LogActivity(string username, string action, string tableAffected, string details)
        {
            string query = $"INSERT INTO AuditLog (Username, Action, TableAffected, Details) " +
                           $"VALUES ('{username}', '{action}', '{tableAffected}', '{details}')";
            ExecuteNonQuery(query);
        }

    }
}
