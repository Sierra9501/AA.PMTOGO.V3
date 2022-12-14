//using AA.PMTOGO.Logging.Implementations;
//using AA.PMTOGO.Models;
//using System.Data.SqlClient;
//using System.Data.SqlTypes;

using AA.PMTOGO.Models;
using System.Data.SqlClient;

namespace AA.PMTOGO.SqlRegistrationDAO
{
    public class RegistrationDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.Users;Trusted_Connection=True;Encrypt=false";
        //private DatabaseLogger _Logger = new DatabaseLogger("business", new SqlLoggerDAO());
        public RegistrationDAO()
        {

        }

        public User FindUser(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "select * from UserAccounts where Username = @username";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.username = (string)reader["Username"];
                        user.password = (string)reader["Password"];
                        user.salt = (byte[])reader["Salt"];
                        user.isActive = (bool)reader["IsActive"];
                        user.attempts = (int)reader["Attempts"];

                        return user;
                    }

                }

            }

            return null;
        }

        public Result DoesUserExist(string username)
        {
            var result = new Result();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "select * from UserAccounts where Username = @username";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (username.Equals(reader["Email"]))
                        {
                            result.IsSuccessful = true;
                            result.ErrorMessage = "Email already exists.";
                            return result;
                        }
                    }
                }

                result.IsSuccessful = false;
                return result;
            }
        }

        public Result DeactivateUser(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccount SET isActive = 0";

                var command = new SqlCommand(sqlQuery, connection);

                try
                {
                    var rows = command.ExecuteNonQuery();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

        public Result ActivateUser(string username)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "UPDATE UserAccount SET isActive = 1";

                var command = new SqlCommand(sqlQuery, connection);

                try
                {
                    var rows = command.ExecuteNonQuery();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        public Result SaveUserAccount(string email, string password, byte[] salt, string role)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "insert into UserAccounts values ( @email, @password, @salt, 1, @role)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@salt", salt);
                command.Parameters.AddWithValue("@role", role);


                try
                {
                    var rows = command.ExecuteNonQuery();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }
        public Result SaveUserProfile(string email, string firstName, string lastName, DateTime dob, int role)
        {
            var result = new Result();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlQuery = "insert into UserProfiles values ( @email, @firstName, @lastName, @dob, @role)";

                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@dob", dob);
                command.Parameters.AddWithValue("@role", role);


                try
                {
                    var rows = command.ExecuteNonQuery();

                    if (rows == 1)
                    {
                        result.IsSuccessful = true;
                        return result;
                    }

                    else
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "too many rows affected";
                        return result;
                    }
                }

                catch (SqlException e)
                {
                    if (e.Number == 208)
                    {
                        // _Logger.AsyncLog("error", "addUser", "Specified table not found.");
                    }
                }

            }

            result.IsSuccessful = false;
            return result;
        }

    }
}
