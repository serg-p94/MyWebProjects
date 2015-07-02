using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcCalculator.Helper
{
    public class HistoryProcessor
    {
        private SqlConnection connection;

        public HistoryProcessor()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["HistoryDB"].ConnectionString);
            connection.Open();
        }

        public void Add(HistoryEntry entry)
        {
            var command = new SqlCommand("INSERT INTO History([v1], [v2], [operation], [ip], [date], [user]) VALUES (@v1, @v2, @operation, @ip, @date, @user)", connection);
            command.Parameters.Add(new SqlParameter("v1", entry.v1));
            command.Parameters.Add(new SqlParameter("v2", entry.v2));
            command.Parameters.Add(new SqlParameter("operation", entry.operation.Value.ToString()));
            command.Parameters.Add(new SqlParameter("ip", entry.ip));
            command.Parameters.Add(new SqlParameter("date", entry.date));
            command.Parameters.Add(new SqlParameter("user", entry.user));
            command.ExecuteNonQuery();
        }

        public void Add(double v1, double v2, Operation operation, HttpContextBase context)
        {
            var entry = new HistoryEntry();
            entry.v1 = v1;
            entry.v2 = v2;
            entry.operation = operation;
            entry.ip = context.Request.ServerVariables["REMOTE_ADDR"];
            entry.date = DateTime.Now;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                entry.user = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                entry.user = null;
            }
            Add(entry);
        }

        public IEnumerable<HistoryEntry> GetHistory(string user)
        {
            SqlCommand command;
            if (user != null)
            {
                command = new SqlCommand("SELECT * FROM History WHERE [user] = @user ORDER BY date DESC", connection);
                command.Parameters.Add(new SqlParameter("user", user));
            }
            else
            {
                command = new SqlCommand("SELECT * FROM History ORDER BY date DESC", connection);
            }
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var entry = new HistoryEntry();
                entry.id = (int)reader["id"];
                entry.v1 = (double)reader["v1"];
                entry.v2 = (double)reader["v2"];
                entry.operation = (Operation)Enum.Parse(typeof(Operation), reader["operation"].ToString());
                entry.ip = reader["ip"].ToString();
                entry.date = (DateTime)reader["date"];
                entry.user = reader["user"].ToString();
                yield return entry;
            }
        }
    }

    public class HistoryEntry
    {
        public double id { get; set; }
        public double v1 { get; set; }
        public double v2 { get; set; }
        public Operation? operation { get; set; }
        public string ip { get; set; }

        public DateTime date { get; set; }

        public string user { get; set; }
    }
}