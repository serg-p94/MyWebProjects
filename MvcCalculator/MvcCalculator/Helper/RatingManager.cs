using System.Web.UI.WebControls;
using MvcCalculator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding.Binders;

namespace MvcCalculator.Helper
{
    public class RatingManager
    {
        private SqlConnection connection;
        public RatingManager()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            connection.Open();
        }

        public void SetMark(string userName, int movieId, int mark)
        {
            if (mark == 0)
            {
                DeleteMark(userName, movieId);
                return;
            }
            var oldMark = GetMark(userName, movieId);
            SqlCommand cmd;
            if (oldMark == 0)
            {
                cmd = new SqlCommand("INSERT INTO MARKS(userId, movieId, mark) VALUES ("
                                     + "(SELECT userId FROM USERS WHERE name = @userName), "
                                     + "@movieId, "
                                     + "@mark)", connection);
            }
            else
            {
                cmd = new SqlCommand("UPDATE MARKS SET mark = @mark WHERE "
                                     + "userId = (SELECT userId FROM USERS WHERE name = @userName) AND "
                                     + "movieId = @movieId",
                    connection);
            }
            cmd.Parameters.Add(new SqlParameter("userName", userName));
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            cmd.Parameters.Add(new SqlParameter("mark", mark));
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("SELECT rating, marksNumber FROM MOVIES WHERE [movieId] = @movieId", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            float rating = 0;
            int marksNumber = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    rating = float.Parse(reader["rating"].ToString());
                    marksNumber = int.Parse(reader["marksNumber"].ToString());
                }
            }
            if (oldMark == 0)
            {
                rating = (rating*marksNumber + mark)/(++marksNumber);
            }
            else
            {
                rating += (mark - oldMark) / (float) marksNumber;
            }
            cmd =
                new SqlCommand(
                    "UPDATE MOVIES SET rating = @rating, marksNumber = @marksNumber WHERE [movieId] = @movieId",
                    connection);
            cmd.Parameters.Add(new SqlParameter("rating", rating));
            cmd.Parameters.Add(new SqlParameter("marksNumber", marksNumber));
            cmd.Parameters.Add((new SqlParameter("movieId", movieId)));
            cmd.ExecuteNonQuery();
        }

        public void DeleteMark(string userName, int movieId)
        {
            var oldMark = GetMark(userName, movieId);
            if (oldMark == 0)
            {
                return;
            }
            var cmd = new SqlCommand("DELETE FROM MARKS WHERE movieId = @movieId AND userId = (SELECT userId FROM USERS WHERE name = @userName)", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            cmd.Parameters.Add(new SqlParameter("userName", userName));
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT rating, marksNumber FROM MOVIES WHERE movieId = @movieId";
            float rating = 0;
            int marksNumber = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    rating = float.Parse(reader["rating"].ToString());
                    marksNumber = int.Parse(reader["marksNumber"].ToString());
                    if (marksNumber > 1)
                    {
                        rating = (rating*marksNumber - oldMark)/(--marksNumber);
                    }
                    else
                    {
                        marksNumber = 0;
                        rating = 0;
                    }
                }
            }
            cmd.CommandText = "UPDATE MOVIES SET rating = @rating, marksNumber = @marksNumber WHERE movieId = @movieId";
            cmd.Parameters.Add(new SqlParameter("rating", rating));
            cmd.Parameters.Add(new SqlParameter("marksNumber", marksNumber));
            cmd.ExecuteNonQuery();
        }

        public int GetMark(string userName, int movieId)
        {
            var command = new SqlCommand("SELECT mark FROM MARKS WHERE "
                + "userId = (SELECT userId FROM USERS WHERE name = @userName) "
                + "AND movieId = @movieId", connection);
            command.Parameters.Add(new SqlParameter("userName", userName));
            command.Parameters.Add(new SqlParameter("movieId", movieId));
            int mark;
            var result = command.ExecuteScalar();
            return result != null && int.TryParse(result.ToString(), out mark) ? mark : 0;
        }

        public float CalculateRating(int movieId)
        {
            float rating = 0;
            int marksNumber = 0;
            var cmd = new SqlCommand("SELECT mark FROM MARKS WHERE [movieId] = @movieId", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    rating += int.Parse(reader["mark"].ToString());
                    marksNumber++;
                }
            }
            if (marksNumber != 0)
            {
                rating /= marksNumber;
            }
            cmd = new SqlCommand("UPDATE MOVIES SET rating = @rating, marksNumber = @marksNumber WHERE [movieId] = @movieId", connection);
            cmd.Parameters.Add(new SqlParameter("rating", rating));
            cmd.Parameters.Add(new SqlParameter("marksNumber", marksNumber));
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            cmd.ExecuteNonQuery();
            return rating;
        }
    }
}