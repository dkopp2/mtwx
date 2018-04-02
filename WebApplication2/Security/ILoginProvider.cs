using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Security
{
    public interface ILoginProvider
    {
        Task<LoginResponse> Login(string userName, string password);
    }

    public class LoginResponse
    {
        public LoginReturnTypes ReturnType { get; set; }
        public string ResponseMessage { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }

    public enum LoginReturnTypes
    {
        Error = -1,                   // Critical Error
        Success = 0,                   // Success
        UserNameUsed = 1,              // Create account
        EmailAddressUsed = 2,          // Create account
        UserNameAndEmailUsed = 3,      // Create account
        InvalidEmailAddress = 4,       // Create account
        InvalidUserName = 5,           // Login
        IncorrectPassword = 6,         // Login & UpdateAccount
        SessionNotDeleted = 7,         // Logout
        InvalidSessionId = 8,          // GetSessionInfo
        InvalidEmail = 9,              // RetrieveUserQuestion & GenerateNewPassword
        InvalidUserId = 10,            // GetUserProfile & UpdateAccount
        InvalidAnswer = 11,            // GenerateNewPassword
        UserNotLoggedOn = 12,          // CheckLogin
        PasswordUsed = 13,             // UpdateAccount
        UpdateNotAllowed = 14,         //UpdateAccount
        InvalidValue = 15,             //Update/Create Account
        ConnectionError = 1000
    };
}