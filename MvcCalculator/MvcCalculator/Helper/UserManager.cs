using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using MvcCalculator.Models;

namespace MvcCalculator.Helper
{
    public class UserManager
    {
        private SqlConnection connection;
        public UserManager()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString);
            connection.Open();
        }

        public UserRegistrationResult Register(User user)
        {
            if (!Exists(user.Name))
            {
                Add(user);
                return UserRegistrationResult.Success;
            }
            return UserRegistrationResult.AlreadyExists;
        }

        public UserValidationResult ValidateUser(string name, string password)
        {
            var command = new SqlCommand("SELECT password FROM USERS WHERE [NAME] = @name", connection);
            command.Parameters.Add(new SqlParameter("name", name));
            var rightPwd = (string)command.ExecuteScalar();
            if (rightPwd != null)
            {
                if (rightPwd.Trim() == password)
                {
                    return UserValidationResult.Success;
                }
                else
                {
                    return UserValidationResult.WrongPassword;
                }
            }
            else
            {
                return UserValidationResult.UserNotFound;
            }
        }

        public User.PersonalInformation GetInfo(int userId)
        {
            var command = new SqlCommand("SELECT [NAME], [EMAIL], [BIRTHDATE] FROM USERS WHERE [userId]=@userId", connection);
            command.Parameters.Add(new SqlParameter("userId", userId));
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var pi = new User.PersonalInformation();
                pi.Name = reader["NAME"].ToString().Trim();
                pi.Email = reader["EMAIL"].ToString().Trim();
                pi.BirthDate = (DateTime)reader["BIRTHDATE"];
                return pi;
            }
            return null;
        }

        public IEnumerable<string> GetAllNames()
        {
            var command = new SqlCommand("SELECT [NAME] FROM USERS", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
                yield return reader["NAME"].ToString();
        }

        public int[] GetUsersIds()
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM USERS", connection);
            var nUsers = int.Parse(cmd.ExecuteScalar().ToString());
            var result = new int[nUsers];
            cmd.CommandText = "SELECT userId FROM USERS";
            using (var reader = cmd.ExecuteReader())
            {
                for (int i = 0; i < nUsers && reader.Read(); i++)
                {
                    result[i] = int.Parse(reader["userId"].ToString());
                }
            }
            return result;
        }

        public int DeleteUser(string name)
        {
            var cmd = new SqlCommand("DELETE FROM USERS WHERE name = @name", connection);
            cmd.Parameters.Add(new SqlParameter("name", name));
            return cmd.ExecuteNonQuery();
        }

        private bool Exists(string name)
        {
            var command = new SqlCommand("SELECT userId FROM USERS WHERE [name] = @name", connection);
            command.Parameters.Add(new SqlParameter("name", name));
            return command.ExecuteScalar() != null;
        }

        private void Add(User user)
        {
            var command =
                new SqlCommand(
                    "INSERT INTO USERS([NAME], [PASSWORD], [EMAIL], [BIRTHDATE]) VALUES (@name, @password, @email, @birthDate)",
                    connection);
            command.Parameters.Add(new SqlParameter("name", user.Name));
            command.Parameters.Add(new SqlParameter("password", user.Password));
            command.Parameters.Add(new SqlParameter("email", user.Information.Email));
            command.Parameters.Add(new SqlParameter("birthDate", user.Information.BirthDate));
            command.ExecuteNonQuery();
        }

        internal bool UserExists(string name)
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM USERS WHERE name = @name", connection);
            cmd.Parameters.Add(new SqlParameter("name", name));
            return int.Parse(cmd.ExecuteScalar().ToString()) > 0;
        }

        public int GetUserId(string name)
        {
            var cmd = new SqlCommand("SELECT userId FROM USERS WHERE name = @name", connection);
            cmd.Parameters.Add(new SqlParameter("name", name));
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public Message SendLoginPwd(string email)
        {
            var msg = new Message();
            var cmd = new SqlCommand("SELECT [NAME], [PASSWORD] FROM USERS WHERE [EMAIL] = @email", connection);
            cmd.Parameters.Add(new SqlParameter("email", email));
            string login;
            string password;
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    msg.Type = Message.MessageType.Error;
                    msg.Header = "Password Recovery";
                    msg.Body = "Reader is empty!";
                    return msg;
                }
                login = reader["name"].ToString();
                password = reader["password"].ToString();
            }
            try
            {
                new EmailManager("puzyrnyi.project@gmail.com", "project011235813").Send(email, "Account Recovery",
                    "Login: " + login + Environment.NewLine
                    + "Password: " + password);
            }
            catch (Exception ex)
            {
                msg.Type = Message.MessageType.Error;
                msg.Header = "Password Recovery";
                msg.Body = ex.Message + Environment.NewLine + email;
                return msg;
            }
            msg.Type = Message.MessageType.Success;
            msg.Header = "Password Recovery";
            msg.Body = "Check your email for login and password.";
            return msg;
        }
    }

    public class User
    {
        private int userId;
        public string Name { get; set; }
        public string Password { get; set; }

        public PersonalInformation Information { get; set; }
        
        public User(string name, string password, string email, DateTime? birthDate)
        {
            this.Name = name;
            this.Password = password;
            Information = new PersonalInformation();
            Information.Name = name;
            Information.Email = email;
            Information.BirthDate = birthDate;
        }

        public int GetMovieMark(int movieId)
        {
            return new RatingManager().GetMark(Name, movieId);
        }

        public class PersonalInformation
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime? BirthDate { get; set; }
            public DateTime? GetAge()
            {
                if (BirthDate.HasValue)
                {
                    return DateTime.Now.AddDays(-BirthDate.Value.Day).AddMonths(-BirthDate.Value.Month).AddYears(-BirthDate.Value.Year);
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public enum UserValidationResult
    {
        Success,
        UserNotFound,
        WrongPassword,
        No
    }

    public enum UserRegistrationResult
    {
        Success,
        AlreadyExists,
        No
    }
}