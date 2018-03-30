using System.Linq;
using System.Threading.Tasks;
using Mtwx.Web.Domain;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Mtwx.Web.Security
{
    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public AuthorizationManager()
        {

        }

        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
            {
                return Nok();
            }

            var resources = context.Resource.ToList();
            var resource = resources.First().Value;
            var action = context.Action.FirstOrDefault()?.Value;

            var claims = context.Principal.Claims.ToList();

            var appUserState = claims.Where(c => c.Type.Equals("userState"))
                                        .Select(x => new AppUserState(x.Value))
                                        .FirstOrDefault();

            if (context.Principal.IsInRole(SecurityResources.Roles.AdminRole))
            {
                return Ok();
            }

            // TO Do authorize different sites here
            /*switch (resource)
            {
                case SecurityResources.Modules.RequestsModule:
                    return CheckRequestsModuleAccess(context, action, appUserState);

            }*/
            return Nok();
        }

        public Task<bool> CheckRequestsModuleAccess(ResourceAuthorizationContext context, string action, AppUserState appUserState)
        {
            // for now, just allow all authenticated and enabled users
            return Eval(context.Principal.Identity.IsAuthenticated);
        }

    }
}