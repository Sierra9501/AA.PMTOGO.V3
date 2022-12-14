//using AA.PMTOGO.Logging.Implementations;
using AA.PMTOGO.Models;
using System.Data.SqlClient;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using MySql.Data.MySqlClient;
namespace AA.PMTOGO.SqlAuthenticationDAO
{
    public class SqlAuthenticateDAO
    {
        private static readonly string _connectionString = @"Server=.\SQLEXPRESS;Database=AA.AuthenticationDB;Trusted_Connection=True;Encrypt=false";

        public Result FindUser(string username, string password)
        {
            Result result = new Result();
            var userAuthenticator = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //User database should have username/ password/ attempts
                var command = new SqlCommand("SELECT * FROM Users WHERE username = @username AND password = @password", connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader["attempts"] == "3")
                    {
                        result.IsSuccessful = false;
                        result.ErrorMessage = "Account is Locked";
                        return result;
                    }
                    if (reader["username"].Equals(username) && reader["password"].Equals(password))
                    {
                        command = new SqlCommand("UPDATE Users SET Attempts = 0", connection);
                        command.ExecuteNonQuery();
                        result.IsSuccessful = true;
                        return result;
                    }
                    result.IsSuccessful = false;
                    result.ErrorMessage = "Incorrect Username or Password";
                    return result;
                }



            }
            /*result.IsSuccessful = true;
            return result;*/
        }

        public void LogFailedAuthenticationAttempts(string username)
        {
            var userAuthenticator = new User();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();


                var command = new SqlCommand("SELECT * FROM Users WHERE username = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                var reader = command.ExecuteReader();
                reader.Read();
                int failedAttempts = (int)reader["Attempts"];
                if (failedAttempts == 0)
                {
                    command = new SqlCommand("UPDATE Users SET Attempts = 1", connection);
                    command.ExecuteNonQuery();

                    command = new SqlCommand("UPDATE Users SET timestamp = CURRENT_TIMESTAMP", connection);
                    command.ExecuteNonQuery();
                }
                else if (failedAttempts == 2)
                {
                    command = new SqlCommand("UPDATE Users SET Attempts = 2", connection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new SqlCommand("UPDATE Users SET Attempts = 3", connection);
                    var rows = command.ExecuteNonQuery();
                    //TODO: log username, Ip, timestamp to database
                }

            }
        }
    }
}
