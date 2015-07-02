using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCalculator.Helper.Oracul;
using MvcCalculator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MvcCalculator.Helper
{
    public class MovieManager
    {
        private SqlConnection connection;

        public MovieManager()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            connection.Open();
        }

        public int Add(Movie mov)
        {
            var command =
                new SqlCommand(
                    "INSERT INTO MOVIES([title], [releaseDate], [genre], [rating], [marksNumber], [description]"
                    + (mov.Cover == null ? ") " : ", [cover]) ")
                    + "VALUES (@title, @releaseDate, @genre, @rating, @marksNumber, @description"
                    + (mov.Cover == null ? ")" : ", @cover)"), connection);
            command.Parameters.Add(new SqlParameter("title", mov.Title));
            command.Parameters.Add(new SqlParameter("releaseDate", mov.ReleaseDate));
            command.Parameters.Add(new SqlParameter("genre", mov.Genre));
            command.Parameters.Add(new SqlParameter("rating", mov.Rating));
            command.Parameters.Add(new SqlParameter("marksNumber", mov.MarksNumber));
            command.Parameters.Add(new SqlParameter("description", mov.Description));
            if (mov.Cover != null)
                command.Parameters.Add(new SqlParameter("cover", mov.Cover));
            command.ExecuteNonQuery();
            command.CommandText = "SELECT MAX(movieId) FROM MOVIES";
            mov.MovieId = int.Parse(command.ExecuteScalar().ToString());
            AddMovieActors(mov.MovieId, mov.Actors);
            return mov.MovieId;
        }

        public void AddCover(int movieId, HttpPostedFileBase postedCover)
        {
            if (postedCover.FileName == string.Empty)
                return;
            var filename = movieId + "_" + postedCover.FileName;
            var cmd = new SqlCommand("UPDATE MOVIES SET cover = @cover WHERE movieId = @movieId", connection);
            cmd.Parameters.Add(new SqlParameter("cover", filename));
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            var coversPath = HttpContext.Current.Server.MapPath(Movie.CoversPathUrl);
            postedCover.SaveAs(coversPath + filename);
            cmd.ExecuteNonQuery();
        }

        public void AddMovieActors(int movieId, IEnumerable<Actor> actors)
        {
            var cmd = new SqlCommand("INSERT INTO MOVIE_ACTOR VALUES (@movieId, @actorId)", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            var actorId = new SqlParameter("actorId", 0);
            cmd.Parameters.Add(actorId);
            foreach (var actor in actors)
            {
                actorId.Value = actor.ActorId;
                cmd.ExecuteNonQuery();
            }
        }

        public Movie GetMovie(int movieId)
        {
            var command = new SqlCommand("SELECT * FROM MOVIES WHERE movieId = @movieId", connection);
            command.Parameters.Add(new SqlParameter("movieId", movieId));
            var mov = new Movie();
            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read())
                    return null;
                mov.MovieId = int.Parse(reader["movieId"].ToString());
                mov.Title = reader["title"].ToString().TrimEnd();
                mov.ReleaseDate = (DateTime) reader["releaseDate"];
                mov.Genre = (MovieGenre) Enum.Parse(typeof (MovieGenre), reader["genre"].ToString());
                mov.Rating = float.Parse(reader["rating"].ToString());
                mov.MarksNumber = int.Parse(reader["marksNumber"].ToString());
                mov.Description = reader["description"].ToString();
                mov.Cover = reader["cover"].ToString().TrimEnd();
            }
            mov.Actors = GetActors(movieId);
            return mov;
        }

        public Actor[] GetActors(int movieId)
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM MOVIE_ACTOR WHERE movieId=@movieId", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            var nActors = int.Parse(cmd.ExecuteScalar().ToString());
            var actors = new Actor[nActors];
            cmd.CommandText =
                "SELECT * FROM ACTORS WHERE actorId IN (SELECT actorId FROM MOVIE_ACTOR WHERE movieId=@movieId)";
            using (var reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < nActors && reader.Read(); i++)
                {
                    var actor = new Actor();
                    actor.ActorId = int.Parse(reader["actorId"].ToString());
                    actor.Name = reader["name"].ToString().TrimEnd();
                    actor.Surname = reader["surname"].ToString().TrimEnd();
                    actors[i] = actor;
                }
            }
            Array.Sort(actors, Actor.CompareAsc);
            return actors;
        }

        public Actor[] GetActorsExcluding(int movieId)
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM ACTORS WHERE actorId NOT IN (SELECT actorId FROM MOVIE_ACTOR WHERE movieId=@movieId)", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            var nActors = int.Parse(cmd.ExecuteScalar().ToString());
            var actors = new Actor[nActors];
            cmd.CommandText =
                "SELECT * FROM ACTORS WHERE actorId NOT IN (SELECT actorId FROM MOVIE_ACTOR WHERE movieId=@movieId)";
            using (var reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < nActors && reader.Read(); i++)
                {
                    var actor = new Actor();
                    actor.ActorId = int.Parse(reader["actorId"].ToString());
                    actor.Name = reader["name"].ToString().TrimEnd();
                    actor.Surname = reader["surname"].ToString().TrimEnd();
                    actors[i] = actor;
                }
            }
            Array.Sort(actors, Actor.CompareAsc);
            return actors;
        }

        public Actor GetActor(string name, string surname)
        {
            var cmd = new SqlCommand("SELECT * FROM ACTORS WHERE [name] = @name AND [surname] = @surname", connection);
            cmd.Parameters.Add(new SqlParameter("name", name));
            cmd.Parameters.Add(new SqlParameter("surname", surname));
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }
                var actor = new Actor();
                actor.ActorId = int.Parse(reader["actorId"].ToString());
                actor.Name = reader["name"].ToString().Trim();
                actor.Surname = reader["surname"].ToString().Trim();
                return actor;
            }
        }

        public bool ActorExists(string name, string surname)
        {
            var cmd = new SqlCommand("SELECT actorId FROM ACTORS WHERE name = @name AND surname = @surname", connection);
            cmd.Parameters.Add(new SqlParameter("name", name));
            cmd.Parameters.Add(new SqlParameter("surname", surname));
            using (var reader = cmd.ExecuteReader())
            {
                return reader.Read();
            }
        }

        public int AddActor(Actor actor)
        {
            var cmd = new SqlCommand("INSERT INTO ACTORS(name, surname) VALUES (@name, @surname)", connection);
            cmd.Parameters.Add(new SqlParameter("name", actor.Name));
            cmd.Parameters.Add(new SqlParameter("surname", actor.Surname));
            return cmd.ExecuteNonQuery();
        }

        public Actor[] ExtractActors(string actors)
        {
            var strs = actors.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            var nActors = strs.Length;
            var result = new Actor[nActors];
            for (int i = 0; i < nActors; i++)
            {
                var surnameName = strs[i].Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries);
                result[i] = GetActor(surnameName[1], surnameName[0]);
            }
            return result;
        }

        public Actor[] GetActors()
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM ACTORS", connection);
            int nActors = int.Parse(cmd.ExecuteScalar().ToString());
            var result = new Actor[nActors];
            cmd.CommandText = "SELECT [actorId], [name], [surname] FROM ACTORS";
            using (var reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < nActors && reader.Read(); i++)
                {
                    result[i] = new Actor();
                    result[i].ActorId = int.Parse(reader["actorId"].ToString());
                    result[i].Name = reader["name"].ToString().TrimEnd();
                    result[i].Surname = reader["surname"].ToString().TrimEnd();
                }
            }
            Array.Sort(result, Actor.CompareAsc);
            return result;
        }

        public Actor[] GetNotAddedActors(int movieId)
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM ACTORS WHERE actorId NOT IN (SELECT actorId FROM MOVIE_ACTOR WHERE movieId = @movieId)", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            int nActors = int.Parse(cmd.ExecuteScalar().ToString());
            var result = new Actor[nActors];
            cmd.CommandText = "SELECT [actorId], [name], [surname] FROM ACTORS WHERE actorId NOT IN (SELECT actorId FROM MOVIE_ACTOR WHERE movieId = @movieId)";
            using (var reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < nActors && reader.Read(); i++)
                {
                    result[i] = new Actor();
                    result[i].ActorId = int.Parse(reader["actorId"].ToString());
                    result[i].Name = reader["name"].ToString().TrimEnd();
                    result[i].Surname = reader["surname"].ToString().TrimEnd();
                }
            }
            Array.Sort(result, Actor.CompareAsc);
            return result;
        }

        private IEnumerable<Movie> GetAll()
        {
            var command = new SqlCommand("SELECT * FROM MOVIES", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var mov = new Movie();
                    mov.MovieId = int.Parse(reader["movieId"].ToString());
                    mov.Title = reader["title"].ToString();
                    mov.ReleaseDate = (DateTime) reader["releaseDate"];
                    mov.Genre = (MovieGenre)Enum.Parse(typeof(MovieGenre), reader["genre"].ToString());
                    mov.Rating = float.Parse(reader["rating"].ToString());
                    mov.MarksNumber = int.Parse(reader["marksNumber"].ToString());
                    mov.Description = reader["description"].ToString();
                    mov.Cover = reader["cover"].ToString();
                    yield return mov;
                }
            }
        }
        public Movie[] GetAll(SortingOptions sort, bool ascending)
        {
            return Sort(GetAll().ToArray(), sort, ascending);
        }

        private Movie[] Sort(Movie[] movs, SortingOptions so, bool ascending)
        {
            switch (so)
            {
                case SortingOptions.Title:
                    if (ascending)
                        Array.Sort(movs, MovieComparator.TitleAscending);
                    else
                        Array.Sort(movs, MovieComparator.TitleDescending);
                    break;
                case SortingOptions.Rating:
                    if (ascending)
                        Array.Sort(movs, MovieComparator.RatingAscending);
                    else
                        Array.Sort(movs, MovieComparator.RatingDescending);
                    break;
                case SortingOptions.ReleaseDate:
                    if (ascending)
                        Array.Sort(movs, MovieComparator.ReleaseDateAscending);
                    else
                        Array.Sort(movs, MovieComparator.ReleaseDateDescending);
                    break;
                case SortingOptions.Oracul:
                {
                    var um = new UserManager();
                    var userIds = um.GetUsersIds();
                    var movieIds = GetMoviesIds();
                    var oracul = new Oracul.Oracul(userIds, movieIds);
                    foreach (var user in userIds)
                    {
                        foreach (var movie in movieIds)
                        {
                            oracul.SetMark(user, movie, GetMark(user, movie));
                        }
                    }
                    return oracul.Sort(movs, um.GetUserId(HttpContext.Current.User.Identity.Name));
                } break;
            }
            return movs;
        }

        private int[] GetMoviesIds()
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM MOVIES", connection);
            var nMovies = int.Parse(cmd.ExecuteScalar().ToString());
            var result = new int[nMovies];
            cmd.CommandText = "SELECT movieId FROM MOVIES";
            using (var reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < nMovies && reader.Read(); i++)
                {
                    result[i] = int.Parse(reader["movieId"].ToString());
                }
            }
            return result;
        }
        
        private short GetMark(int userId, int movieId)
        {
            var cmd = new SqlCommand("SELECT mark FROM MARKS WHERE userId = @userId AND movieId = @movieId", connection);
            cmd.Parameters.Add(new SqlParameter("userId", userId));
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            var res = cmd.ExecuteScalar();
            return res == null ? (short) 0 : short.Parse(res.ToString());
        }

        public static class MovieComparator
        {
            public static int TitleAscending(Movie m1, Movie m2)
            {
                return m1.Title.CompareTo(m2.Title);
            }

            public static int TitleDescending(Movie m1, Movie m2)
            {
                return -TitleAscending(m1, m2);
            }

            public static int ReleaseDateAscending(Movie m1, Movie m2)
            {
                return m1.ReleaseDate.CompareTo(m2.ReleaseDate);
            }

            public static int ReleaseDateDescending(Movie m1, Movie m2)
            {
                return -ReleaseDateAscending(m1, m2);
            }
            public static int RatingAscending(Movie m1, Movie m2)
            {
                return m1.Rating.CompareTo(m2.Rating);
            }

            public static int RatingDescending(Movie m1, Movie m2)
            {
                return -RatingAscending(m1, m2);
            }
        }

        public string GetCoverUrl(int movieId)
        {
            var cmd = new SqlCommand("SELECT cover FROM MOVIES WHERE [movieId]=@movieId", connection);
            cmd.Parameters.Add(new SqlParameter("movieId", movieId));
            return cmd.ExecuteScalar().ToString();
        }
    }

    public enum ActorAddingResult
    {
        Success,
        AlreadyExists,
        No
    }

    public enum SortingOptions
    {
        Rating,
        ReleaseDate,
        Title,
        Oracul,
        No
    }
}