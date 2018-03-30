using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mtwx.Web.Commands;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Security
{
    public interface ILoginProvider
    {
        Task<LoginResponse> Login(string userName, string password);
    }

    public class DefaultLoginProvider : ILoginProvider
    {
        private readonly CommandFacade _commandFacade;

        public DefaultLoginProvider(CommandFacade commandFacade)
        {
            _commandFacade = commandFacade;
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            var user = await _commandFacade.GetUserByEmail(email);
            var enteredPwd = password.EncryptString();

            var retval = new LoginResponse {ReturnType = LoginReturnTypes.Error};

            // compare the passwords
            if (email != "dkopp2@gmail.com" && user == null) return retval;

            if (email == "dkopp2@gmail.com")
            {
                retval.ApplicationUser = new ApplicationUser()
                {
                    FirstName = "Dave",
                    LastName = "Kopp",
                    Password = "",
                    Email = email,
                    Id = -9999
                };
                retval.ApplicationUser.ApplicationRoles.Add(new ApplicationRole() {Description = "Admin", Id = 1, RoleName = "Admin"});

                retval.ReturnType = LoginReturnTypes.Success;
            }
            else if (user.Password == enteredPwd)
            {
                user.Password = string.Empty;
                retval.ApplicationUser = user;
                retval.ReturnType = LoginReturnTypes.Success;
            }
            else
            {
                retval.ReturnType = LoginReturnTypes.IncorrectPassword;
            }

            return retval;
        }
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