
//using AA.PMTOGO.Authenticator;
//using AA.PMTOGO.Authorizor;
//using AA.PMTOGO.Registrator;
//using System.Security;
using AA.PMTOGO.Authentication;
using AA.PMTOGO.Models;
using AA.PMTOGO.Registration;
using System.Security.Principal;
using System.Text.RegularExpressions;
//using System.Security.Claims;
//using Azure.Identity;
//using System.Data;

namespace AA.PMTOGO;
public class App_v1
{
    private Result _result = new Result();
    //private Authorization _authZ;

    public void Run()
    {
        // Get the current principal
        IPrincipal principal = System.Threading.Thread.CurrentPrincipal;

        if (System.Threading.Thread.CurrentPrincipal != null)
        {
            // Check if the user is authenticated
            if (principal.Identity.IsAuthenticated)
            {
                AuthenticatedMenu();
            }
            else
            {
                UnauthenticatedMenu();
            }
        }
        else
        {
            UnauthenticatedMenu();
        }
    }
    private bool Login()
    {
        Authenticator _authN = new Authenticator();
        string emailPattern = @"^[A-Za-z0-9.-]+@[A-Za-z0-9.-]+$";
        string passwordPattern = @"^[ a-zA-Z0-9.,@!-]{8,32}$";
        string username;
        string password;


        // Keep prompting the user for input until their input matches the regex pattern
        System.Console.WriteLine("Enter valid email: ");
        while (!Regex.IsMatch(username = Console.ReadLine(), emailPattern))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Enter valid email: ");
        }

        System.Console.WriteLine("Enter valid password: ");
        while (!Regex.IsMatch(password = Console.ReadLine(), passwordPattern))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Enter valid password: ");
        }
        Result result = _authN.Authenticate(username, password);
        if (result.IsSuccessful)
        {
            var otp = _authN.GenerateOTP();
            System.Console.WriteLine("OTP is " + otp);
            var userInput = System.Console.ReadLine();
            if (userInput == otp)
            {
                string[] userRole = { "Admin" };
                IIdentity userIdentity = new GenericIdentity(username);
                IPrincipal userPrincipal = new GenericPrincipal(userIdentity, userRole);

                Thread.CurrentPrincipal = userPrincipal;
                return true;
            }
            else
            {
                System.Console.WriteLine("Invalid OTP");
            }
        }
        else
        {
            System.Console.WriteLine(result.ErrorMessage);
        }
        _authN.FailedAuthenticationAttempt(username);
        return false;
    }

    private void AuthenticatedMenu()
    {
        string answer;
        do
        {
            System.Console.WriteLine();
            System.Console.WriteLine("1) Request access to view");
            System.Console.WriteLine("2) Request access to view ");
            System.Console.WriteLine("3) Logout ");
            System.Console.WriteLine();

            answer = System.Console.ReadLine();

            switch (answer)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                default:
                    System.Console.WriteLine("Invalid Input");
                    break;
            }


        } while (!answer.Equals("3"));

        System.Console.WriteLine("Logout User");
    }
    private void UnauthenticatedMenu()
    {
        string answer;
        do
        {
            System.Console.WriteLine();
            System.Console.WriteLine("1) Login ");
            System.Console.WriteLine("2) Register User ");
            System.Console.WriteLine("3) Exit Program ");
            System.Console.WriteLine();

            answer = System.Console.ReadLine();

            switch (answer)
            {
                case "1":
                    var login = Login();
                    if (login)
                    {
                        AuthenticatedMenu();
                    }
                    break;
                case "2":
                    var register = Register();
                    break;
                case "3":
                    break;
                default:
                    System.Console.WriteLine("Invalid Input");
                    break;
            }

        } while (!answer.Equals("3"));

        System.Console.WriteLine("Exiting the System");
        System.Environment.Exit(1);
    }

    private bool Register()
    {
        Registrator registration = new Registrator();
        string emailPattern = @"^[A-Za-z0-9.-]+@[A-Za-z0-9.-]+$";
        string passwordPattern = @"^[ a-zA-Z0-9.,@!-]{8,32}$";
        string username;
        string password;
        string role;
        string age;
        string state;

        // Keep prompting the user for input until their input matches the regex pattern
        System.Console.WriteLine("Enter valid email: ");
        while (!Regex.IsMatch(username = Console.ReadLine(), emailPattern))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Enter valid email: ");
        }

        System.Console.WriteLine("Enter valid password: ");
        while (!Regex.IsMatch(password = Console.ReadLine(), passwordPattern))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Enter valid password: ");
        }
        System.Console.WriteLine("Choose account type: \n1) Property Manager \n2) Service Providers \n  ");
        role = Console.ReadLine();
        while (!(role.Equals("1") || role.Equals("2")))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Choose account type: \n1) Property Manager \n2) Service Providers \n  ");
            role = Console.ReadLine();
        }
        System.Console.WriteLine("Are you over 13: \n1) Yes \n2) No \n ");
        while (!((age = Console.ReadLine()).Equals("1")))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Are you over 13: \n1) Yes \n2) No \n ");
        }
        System.Console.WriteLine("Are you from califorina: \n1) Yes \n2) No \n");
        while (!((state = Console.ReadLine()).Equals("1")))
        {
            Console.WriteLine("Invalid input. Please try again.");
            System.Console.WriteLine("Are you from califorina: \n1) Yes \n2) No \n");
        }
        byte[] salt = registration.GenerateSalt();
        byte[] pass = registration.EncrpytPassword(password, salt);

        registration.CreateUser(username, pass, salt, role);

        return false;
    }

    public Result ValidateAccountType(int role)
    {
        var result = new Result();

        if (role == 1 || role == 2)
        {
            result.IsSuccessful = true;
            return result;
        }
        else
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid Account Type. Retry again.";
            return result;
        }

    }
    public Result ValidateEmail(string email)
    {
        var result = new Result();

        if (email == null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid email provided. Retry again or contact system administrator";
            return result;
        }
        if (Regex.IsMatch(email, @"^[a-zA-Z0-9-@.\s]+$") && email.Length >= 8 && email.Length < 30)
        {
            result.IsSuccessful = true;
            return result;
        }

        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid email provided. Retry again or contact system administrator";
        return result;
    }
    public Result ValidatePassword(string passWord)
    {
        var result = new Result();
        if (passWord == null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
            return result;
        }
        if (Regex.IsMatch(passWord, @"^[a-zA-Z0-9-@.\s]+$") && passWord.Length >= 8 && passWord.Length < 30)
        {
            result.IsSuccessful = true;
            return result;
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid passphrase provided. Retry again or contact system administrator";
        return result;
    }
    public Result ValidateName(string name)
    {
        var result = new Result();
        if (name == null)
        {
            result.IsSuccessful = false;
            result.ErrorMessage = "Invalid";
            return result;
        }
        if (Regex.IsMatch(name, @"^[a-zA-Z0-9-\s]+$") && name.Length >= 8 && name.Length < 30)
        {
            result.IsSuccessful = true;
            return result;
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Cannot use Special Characters";
        return result;
    }
    public Result ValidateDateOfBirth(DateTime dob)
    {
        var result = new Result();
        if ((DateTime.Now - dob).Ticks >= 13)
        {
            result.IsSuccessful = true;
            return result;
        }
        result.IsSuccessful = false;
        result.ErrorMessage = "Invalid age. User must be 13 years old or older.";
        return result;
    }
}