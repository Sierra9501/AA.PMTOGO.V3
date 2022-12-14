
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using AA.PMTOGO.Authentication;
//using AA.PMTOGO.Authentications;

namespace AA.PMTOGO.AuthenticationTest
{
    [TestClass]
    public class AuthenticationUnitTest
    {
        public void ShouldCreateInstanceWithDefaultCtor()
        {
            // Arrange
            var expected = typeof(Authenticator);

            // Act
            var actual = new Authenticator();

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType() == expected);
        }

        // The user must provide valid security credentials whenever attempting to authenticate with the system

        // Valid username
        [TestMethod]
        public void ShouldProvideValidUsername()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkValidUsername = authenticator.ValidateUsername("Abc.-@123");
            bool checkInvalidMinimumUsernameLength = authenticator.ValidateUsername("abc123");
            bool checkInvalidMaximumUsernameLength = authenticator.ValidateUsername("abcdefghijklmnopqrstuvwxyz01234567890abcdefghijklmnopqrstuvwxyz123");
            bool checkInvalidUsernameCharacters = authenticator.ValidateUsername("Abc.-@123]");
            bool checkBlankUsername = authenticator.ValidateUsername(null);

            // Assert
            Assert.IsNotNull(checkValidUsername);
            Assert.IsNotNull(checkInvalidMinimumUsernameLength);
            Assert.IsNotNull(checkInvalidMaximumUsernameLength);
            Assert.IsNotNull(checkInvalidUsernameCharacters);
            Assert.IsNotNull(checkBlankUsername);

            Assert.IsTrue(checkValidUsername);
            Assert.IsFalse(checkInvalidMinimumUsernameLength);
            Assert.IsFalse(checkInvalidMaximumUsernameLength);
            Assert.IsFalse(checkInvalidUsernameCharacters);
            Assert.IsFalse(checkBlankUsername);
        }

        // Valid password
        [TestMethod]
        public void ShouldProvideValidPassphrase()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkValidPassphrase = authenticator.ValidatePassword("aZ09  .,@!").IsSuccessful;
            bool checkPassphraseNotValidCharacters = authenticator.ValidatePassword("[apple]").IsSuccessful;
            bool checkPassphraseNotValidlength = authenticator.ValidatePassword("pass123").IsSuccessful;


            // Assert
            Assert.IsNotNull(checkValidPassphrase);
            Assert.IsNotNull(checkPassphraseNotValidCharacters);
            Assert.IsNotNull(checkPassphraseNotValidlength);
            Assert.IsTrue(checkValidPassphrase);
            Assert.IsFalse(checkPassphraseNotValidCharacters);
            Assert.IsFalse(checkPassphraseNotValidlength);
        }

        //Valid OTP
        [TestMethod]
        public void ShouldProvideValidOTP()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkValidOTP = authenticator.CheckValidOTP("abcABC123");
            bool checkInvalidMinimumOTPLength = authenticator.CheckValidOTP("abc123");
            bool checkInvalidMaximumOTPLength = authenticator.CheckValidOTP("abcdefghijklmnopqrstuvwxyz01234567890abcdefghijklmnopqrstuvwxyz123");
            bool checkInvalidOTPCharacters = authenticator.CheckValidOTP("abcABC.123");
            bool checkBlankOTP = authenticator.CheckValidOTP(null);

            // Assert
            Assert.IsNotNull(checkValidOTP);
            Assert.IsNotNull(checkInvalidMinimumOTPLength);
            Assert.IsNotNull(checkInvalidMaximumOTPLength);
            Assert.IsNotNull(checkInvalidOTPCharacters);
            Assert.IsNotNull(checkBlankOTP);

            Assert.IsTrue(checkValidOTP);
            Assert.IsFalse(checkInvalidMinimumOTPLength);
            Assert.IsFalse(checkInvalidMaximumOTPLength);
            Assert.IsFalse(checkInvalidOTPCharacters);
            Assert.IsFalse(checkBlankOTP);
        }

        [TestMethod]
        public void ShouldGenerateValidOTP()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            var checkGenerateOTP = authenticator.GenerateOTP("username@gmail.com");


            // Assert
            Assert.IsNotNull(checkGenerateOTP);
            Assert.IsTrue(checkGenerateOTP.Length >= 8);
            Assert.IsTrue(checkGenerateOTP.Length <= 64);
            Assert.IsTrue(Regex.IsMatch(checkGenerateOTP, @"^[a-zA-Z0-9\s]+$"));
        }

        [TestMethod]
        public void ShouldCheckIfUserSessionIsAuthenticated()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkUserSessionNotAuthentication = authenticator.CheckUserSessionAthentication();
            var generateOTP = authenticator.generateOTP("username@gmail.com");
            bool authenticateUser = authenticator.AuthenticateUser("Username@gmail.com", generateOTP);
            bool checkUserSessionIsAuthentication = authenticator.CheckUserSessionAthentication();

            // Assert
            Assert.IsNotNull(checkUserSessionNotAuthentication);
            Assert.IsNotNull(generateOTP);
            Assert.IsNotNull(authenticateUser);
            Assert.IsNotNull(checkUserSessionIsAuthentication);

            Assert.IsTrue(checkUserSessionNotAuthentication);
            Assert.IsTrue(authenticateUser);
            Assert.IsFalse(checkUserSessionIsAuthentication);
        }


        [TestMethod]
        public void ShouldAuthenticateUser()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            var generateOTP = authenticator.generateOTP("username@gmail.com");
            bool checkValidUserAuthentication = authenticator.AuthenticateUser("username@gmail.com", generateOTP);
            bool checkInvalidUsernameAuthentication = authenticator.AuthenticateUser("abc", generateOTP);
            bool checkInvalidOTPAuthentication = authenticator.AuthenticateUser("username@gmail.com", "abc");
            bool checkNullUserAuthentication = authenticator.AuthenticateUser(null, null);

            // Assert
            Assert.IsNotNull(generateOTP);
            Assert.IsNotNull(checkValidUserAuthentication);
            Assert.IsNotNull(checkInvalidUsernameAuthentication);
            Assert.IsNotNull(checkInvalidOTPAuthentication);
            Assert.IsNotNull(checkNullUserAuthentication);

            Assert.IsTrue(checkValidUserAuthentication);
            Assert.IsFalse(checkInvalidUsernameAuthentication);
            Assert.IsFalse(checkInvalidOTPAuthentication);
            Assert.IsFalse(checkNullUserAuthentication);
        }

        [TestMethod]
        public void ShouldLockAccount()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            bool checkIfAccountIsNotLocked = authenticator.LockAccount("Username@gmail.com");
            bool failAuthenticationFirstTime = authenticator.AuthenticateUser("Username@gmail.com", "abc");
            bool failAuthenticationSecondTime = authenticator.AuthenticateUser("Username@gmail.com", "abc");
            bool failAuthenticationThirdTime = authenticator.AuthenticateUser("Username@gmail.com", "abc");
            bool checkIfAccountIsLocked = authenticator.LockAccount("Username@gmail.com");

            // Assert
            Assert.IsNotNull(checkIfAccountIsNotLocked);
            Assert.IsNotNull(failAuthenticationFirstTime);
            Assert.IsNotNull(failAuthenticationSecondTime);
            Assert.IsNotNull(failAuthenticationThirdTime);
            Assert.IsNotNull(checkIfAccountIsLocked);

            Assert.IsFalse(checkIfAccountIsNotLocked);
            Assert.IsFalse(failAuthenticationFirstTime);
            Assert.IsFalse(failAuthenticationSecondTime);
            Assert.IsFalse(failAuthenticationThirdTime);
            Assert.IsTrue(checkIfAccountIsLocked);
        }

        [TestMethod]
        public void ShouldChangeOTPAfterUse()
        {
            // Arrange
            var authenticator = new Authenticator();
            var generateOTP = authenticator.generateOTP("username@gmail.com");
            bool authenticateUser = authenticator.AuthenticateUser("Username@gmail.com", generateOTP);

            // Act
            bool authenticateUserWithSameOTP = authenticator.AuthenticateUser("Username@gmail.com", generateOTP);

            // Assert
            Assert.IsNotNull(generateOTP);
            Assert.IsNotNull(authenticateUser);
            Assert.IsNotNull(authenticateUserWithSameOTP);

            Assert.IsTrue(authenticateUser);
            Assert.IsTrue(authenticateUserWithSameOTP);

        }

        [TestMethod]
        public void ShouldHaveOTPExpireIn2Minutes()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            var generateOTP = authenticator.generateOTP("username@gmail.com");
            Task.Delay(120);
            bool authenticateUser = authenticator.AuthenticateUser("Username@gmail.com", generateOTP);

            // Assert
            Assert.IsNotNull(generateOTP);
            Assert.IsNotNull(authenticateUser);

            Assert.IsFalse(authenticateUser);
        }

        [TestMethod]
        public void ShouldSetLockTimer()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            var checkTimer = authenticator.LockTimer("Username@gmail.com", false);

            // Assert
            Assert.IsNotNull(checkTimer);

            Assert.IsTrue(checkTimer <= 86400);
        }

        public void ShouldResetLockTimer()
        {
            // Arrange
            var authenticator = new Authenticator();

            // Act
            var checkTimer = authenticator.LockTimer("Username@gmail.com", true);

            // Assert
            Assert.IsNotNull(checkTimer);

            Assert.IsTrue(checkTimer == 86400);
        }
    }
}
