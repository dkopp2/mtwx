using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Commands
{
    public class CommandFacade
    {
        private readonly IMediator _mediator;

        public CommandFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ApplicationUser> GetApplicationUserByUserName(string userName)
        {
            // get the user
            var userCmd = new GetApplicationUserByUserName(userName);
            var applicationUserEntity = await _mediator.Send(userCmd);

            return new ApplicationUser()
            {
                Id = applicationUserEntity.Id,
                Email = applicationUserEntity.Email,
                Password = applicationUserEntity.Password,
                FirstName = applicationUserEntity.FirstName,
                LastName = applicationUserEntity.LastName
            };
        }

        public async Task<IEnumerable<ApplicationRole>> GetApplicationUserRoles(int userId)
        {
            var cmd = new GetApplicationUserRoles(userId);
            var userRoles = await _mediator.Send(cmd);

            return userRoles.Select(r => new ApplicationRole()
            {
                Id = r.Id,
                Description = r.Description,
                RoleName = r.RoleName
            });
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRole(string roleName)
        {
            var cmd = new GetUsersInRole(roleName);
            var users = await _mediator.Send(cmd);

            return users.Select(r => new ApplicationUser()
            {
                Id = r.Id,
                Email = r.Email,
                Password = r.Password,
                FirstName = r.FirstName,
                LastName = r.LastName
            });
        }

        public async Task<IEnumerable<ApplicationRole>> GetApplicationRoleList()
        {
            var cmd = new GetApplicationRoleList();
            var roles = await _mediator.Send(cmd);

            return roles.Select(r => new ApplicationRole()
            {
                RoleName = r.RoleName,
                Id = r.Id,
                Description = r.Description
            });
        }

        public async Task<IEnumerable<ApplicationRole>> GetUserRoles(string userName)
        {
            var cmd = new GetUserRoles(userName);
            var userRoles = await _mediator.Send(cmd);

            return userRoles.Select(r => new ApplicationRole()
            {
                RoleName = r.RoleName,
                Id = r.Id,
                Description = r.Description
            });
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleByRoleId(int roleId)
        {
            var cmd = new GetUsersInRoleByRoleId(roleId);
            var roles = await _mediator.Send(cmd);

            return roles.Select(r => new ApplicationUser()
            {
                Id = r.Id,
                Email = r.Email,
                Password = r.Password,
                FirstName = r.FirstName,
                LastName = r.LastName
            });
        }

        public async Task<ApplicationRole> GetApplicationRole(int roleId)
        {
            var cmd = new GetApplicationRole(roleId);
            var role = await _mediator.Send(cmd);

            return new ApplicationRole { Id = role.Id, RoleName = role.RoleName, Description = role.Description };
        }

        public async Task<int> CreateApplicationRole(string roleName, string description)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var cmd = new CreateApplicationRole(roleName, description, userName);
            return await _mediator.Send(cmd);
        }

        public async Task<int> UpdateApplicationRole(int id, string roleName, string description)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var cmd = new UpdateApplicationRole(id, roleName, description, userName);
            return await _mediator.Send(cmd);
        }

        public async Task<int> DeleteApplicationRole(int id)
        {
            var cmd = new DeleteApplicationRole(id);
            return await _mediator.Send(cmd);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var cmd = new GetUsers();
            var users = await _mediator.Send(cmd);

            return users.Select(r => new ApplicationUser()
            {
                Id = r.Id,
                Email = r.Email,
                Password = r.Password,
                LastName = r.LastName,
                FirstName = r.FirstName
            });
        }

        public async Task<ApplicationUser> GetApplicationUser(int id)
        {
            var cmd = new GetApplicationUser(id);
            var user = await _mediator.Send(cmd);

            return new ApplicationUser()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                LastName = user.LastName,
                FirstName = user.FirstName
            };
        }

        public async Task<int> CreateApplicationUser(string email, string password, string firstName, string lastName)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var cmd = new CreateApplicationUser(email, password, firstName, lastName, userName);
            return await _mediator.Send(cmd);
        }

        public async Task<int> ClearUserRoles(int userId)
        {
            var cmd = new ClearUserRoles(userId);
            return await _mediator.Send(cmd);
        }

        public async Task<int> AddRolesToUser(int userId, List<int> roleList)
        {
            var cmd = new AddRolesToUser(userId, roleList);
            return await _mediator.Send(cmd);
        }

        public async Task<int> DeleteApplicationUser(int userId)
        {
            var cmd = new DeleteApplicationUser(userId);
            return await _mediator.Send(cmd);
        }

        public async Task<int> UpdateApplicationUser(ApplicationUser user)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var cmd = new UpdateApplicationUser(user.Id, user.Email, user.Password, user.FirstName, user.LastName, userName);
            return await _mediator.Send(cmd);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var cmd = new GetUserByEmail(email);
            var result = await _mediator.Send(cmd);

            if (result == null) return null;

            var rolesCmd = new GetApplicationUserRoles(result.Id);
            var roles = await _mediator.Send(rolesCmd);

            var retval = new ApplicationUser
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                Password = result.Password,
                Id = result.Id
            };

           foreach (var r in roles) {
                retval.ApplicationRoles.Add(
                new ApplicationRole()
                {
                    Id = r.Id,
                    Description = r.Description,
                    RoleName = r.RoleName
                });
            };

            return retval;
        }

    }
}