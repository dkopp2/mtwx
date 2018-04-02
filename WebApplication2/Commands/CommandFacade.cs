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

            var cmd2 = new GetApplicationRoleExternalSiteList(roleId);
            var sites = await _mediator.Send(cmd2);

            var retval =  new ApplicationRole { Id = role.Id, RoleName = role.RoleName, Description = role.Description };
            foreach (var site in sites)
            {
                retval.ExternalSites.Add(new ExternalSite()
                {
                    Description = site.Description,
                    Id = site.Id,
                    Name = site.Name,
                    FormId = site.FormId,
                    Href = site.Href,
                    FormPasswordField = site.FormPasswordField,
                    FormUserIdField = site.FormUserIdField,
                    SiteUserId = site.SiteUserId,
                    LoginAction = site.LoginAction,
                    SitePassword = site.SitePassword
                });
            }

            return retval;
        }

        public async Task CreateApplicationRole(ApplicationRole role, string userName)
        {
            var cmd = new CreateApplicationRole(role.RoleName,role.Description, userName);
            var roleEntity = await _mediator.Send(cmd);

            var cmd3 = new SetRoleExternalSites(roleEntity.Id, role.ExternalSites.Select(x => x.Id));
            await _mediator.Send(cmd3);
        }

        public async Task<int> UpdateApplicationRole(ApplicationRole role, string userName)
        {
            var cmd = new UpdateApplicationRole(role.Id, role.RoleName, role.Description, userName);
            await _mediator.Send(cmd);

            var cmd3 = new SetRoleExternalSites(role.Id, role.ExternalSites.Select(x => x.Id));
            return await _mediator.Send(cmd3);
        }

        public async Task<int> DeleteApplicationRole(int id)
        {
            var cmd = new DeleteApplicationRole(id);
            return await _mediator.Send(cmd);
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicationUserList()
        {
            var cmd = new GetApplicationUserList();
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

            var rolesCmd = new GetApplicationUserRoles(user.Id);
            var roles = await _mediator.Send(rolesCmd);

            var sitesCmd = new GetUserExternalSiteList(user.Id);
            var sites = await _mediator.Send(sitesCmd);

            var retval = new ApplicationUser()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                LastName = user.LastName,
                FirstName = user.FirstName
            };

            foreach (var role in roles)
            {
                retval.ApplicationRoles.Add(new ApplicationRole() {
                    Description = role.Description,
                    Id = role.Id,
                    RoleName = role.RoleName
                });
            }

            foreach (var site in sites)
            {
                retval.ExternalSites.Add(new ExternalSite()
                {
                    Description = site.Description,
                    Id = site.Id,
                    Name = site.Name,
                    FormId = site.FormId,
                    Href = site.Href,
                    FormPasswordField = site.FormPasswordField,
                    FormUserIdField = site.FormUserIdField,
                    SiteUserId = site.SiteUserId,
                    LoginAction = site.LoginAction,
                    SitePassword = site.SitePassword
                });
            }

            return retval;
        }

        public async Task CreateApplicationUser(ApplicationUser user, string userName)
        {
            // var userName = HttpContext.Current.User.Identity.Name;
            var cmd = new CreateApplicationUser(user.Email, user.Password, user.FirstName, user.LastName, userName);
            var newUser =  await _mediator.Send(cmd);

            var cmd2 = new SetUserApplicationRoles(newUser.Id, user.ApplicationRoles.Select(x => x.Id));
            await _mediator.Send(cmd2);

            var cmd3 = new SetUserExternalSites(newUser.Id, user.ExternalSites.Select(x => x.Id));
            await _mediator.Send(cmd3);

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

        public async Task<int> DeleteApplicationUser(int id)
        {
            var cmd = new DeleteApplicationUser(id);
            return await _mediator.Send(cmd);
        }

        public async Task<int> UpdateApplicationUser(ApplicationUser user, string userName)
        {
            var cmd = new UpdateApplicationUser(user.Id, user.Email, user.Password, user.FirstName, user.LastName, userName);
            await _mediator.Send(cmd);

            var cmd2 = new SetUserApplicationRoles(user.Id, user.ApplicationRoles.Select(x => x.Id));
            await _mediator.Send(cmd2);

            var cmd3 = new SetUserExternalSites(user.Id, user.ExternalSites.Select(x => x.Id));
            return await _mediator.Send(cmd3);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var cmd = new GetUserByEmail(email);
            var result = await _mediator.Send(cmd);

            if (result == null) return null;

            var rolesCmd = new GetApplicationUserRoles(result.Id);
            var roles = await _mediator.Send(rolesCmd);

            var sitesCmd = new GetUserExternalSiteList(result.Id);
            var sites = await _mediator.Send(sitesCmd);

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
            }

            foreach (var s in sites)
            {
                retval.ExternalSites.Add(new ExternalSite()
                {
                    Name = s.Name,
                    Id = s.Id,
                    Description = s.Description,
                    SiteUserId = s.SiteUserId,
                    SitePassword = s.SitePassword,
                    FormId = s.FormId,
                    Href = s.Href,
                    FormUserIdField = s.FormUserIdField,
                    FormPasswordField = s.FormPasswordField,
                    LoginAction = s.LoginAction
                });
            }

            return retval;
        }

        public async Task<IEnumerable<ExternalSite>> GetExternalSiteList()
        {
            var cmd = new GetExternalSiteList();
            var results = await _mediator.Send(cmd);

            return results.Select(x => new ExternalSite()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                FormId = x.FormId,
                FormPasswordField = x.FormPasswordField,
                FormUserIdField = x.FormUserIdField,
                Href = x.Href,
                LoginAction = x.LoginAction,
                SitePassword = x.SitePassword,
                SiteUserId = x.SiteUserId
            });
        }

        public async Task<ExternalSite> GetExternalSite(int id)
        {
            var cmd = new GetExternalSite(id);
            var result = await _mediator.Send(cmd);

            return  new ExternalSite()
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                FormId = result.FormId,
                FormPasswordField = result.FormPasswordField,
                FormUserIdField = result.FormUserIdField,
                Href = result.Href,
                LoginAction = result.LoginAction,
                SitePassword = result.SitePassword,
                SiteUserId = result.SiteUserId
            };
        }

        public async Task<int> CreateExternalSite(ExternalSite model, string createdBy)
        {
            var cmd = new CreateExternalSite(model.Name,model.Description, model.FormId, model.Href, model.LoginAction, model.SiteUserId, model.SitePassword, model.FormUserIdField, model.FormPasswordField, createdBy);
            return await _mediator.Send(cmd);
        }

        public async Task<int> UpdateExternalSite(ExternalSite model, string updatedBy)
        {
            var cmd = new UpdateExternalSite(model.Name, model.Description, model.FormId, model.Href, model.LoginAction, model.SiteUserId, model.SitePassword, model.FormUserIdField, model.FormPasswordField, model.Id, updatedBy);
            return await _mediator.Send(cmd);
        }

        public async Task<int> DeleteExternalSite(int id)
        {
            var cmd = new DeleteExternalSite(id);
            return await _mediator.Send(cmd);
        }

    }
}