using AA.PMTOGO.Models;
using AA.PMTOGO.SqlAuthenticationDAO;

//using System.Text.RegularExpressions;
namespace AA.PMTOGO.Authentication
{
    public class Authenticator
    {

        SqlAuthenticateDAO _authNDAO = new SqlAuthenticateDAO();

        public Result Authenticate(string username, string password)
        {
            Result result = new Result();
            result = _authNDAO.Authenticate(username, password);

            return result;
        }

        public void FailedAuthenticationAttempt(string username)
        {
            _authNDAO.LogFailedAuthenticationAttempts(username);
        }

        public string GenerateOTP()
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.-@";
            Random rand = new Random();
            string otp = "";
            for (int i = 0; i < 8; i++)
            {
                otp += allowedChars[rand.Next(0, allowedChars.Length)];
            }
            return otp;
        }
    }
}
