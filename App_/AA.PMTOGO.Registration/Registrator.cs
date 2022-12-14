using System.Security.Cryptography;
using System.Text;
//using AA.PMTOGO.Logging.Implementations;
using AA.PMTOGO.Models;
using AA.PMTOGO.SqlRegistrationDAO;
namespace AA.PMTOGO.Registration
{
    public class Registrator
    {
        //private DatabaseLogger _Logger = new DatabaseLogger("business", new SqlLoggerDAO());

        //private Sql registrationDAO = new SqlRegistrationDAO();

        public Registration()
        {

        }
        public byte[] GenerateSalt()
        {
            string salt = " ";

            Random rand = new Random();
            salt = (rand.Next(100000, 999999)).ToString();

            //userAuthenticator.IsSuccessful = true;
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            return saltBytes;
        }

        public byte[] EncrpytPassword(string password, byte[] salt)
        {

            var pass = Encoding.UTF8.GetBytes(password);
            // Lecture Vong 12/13 
            var hash = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512);
            var encryptedPass = hash.GetBytes(64);

            return encryptedPass;
        }

        public Result CreateUser(string email, string password, string role)
        {
            RegistrationDAO registrationDAO = new RegistrationDAO();
            var result = new Result();
            var user = registrationDAO.FindUser(email);

            if (user == null)
            {
                var salt = GenerateSalt();
                var password_digest = EncrpytPassword(password, salt);

                //add user account
                registrationDAO.SaveUserAccount(email, password, salt, role);
                //_Logger.Log("info", "CreateUserAccount", "Succesfully created an user");

                //add user profile
                //registrationDAO.SaveUserProfile(email, firstname, lastname, dob, role);
                //_Logger.Log("info", "CreateUserProfile", "Succesfully created an user");
                result.IsSuccessful = true;
                return result;
            }
            else
            {
                result.ErrorMessage = "User account already exists.";
                result.IsSuccessful = false;
                return result;
            }
        }
    }

