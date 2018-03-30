using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Mtwx.Web.Commands;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Controllers
{
    public class BaseController : Controller
    {


        protected AppUserState AppUserState { get; private set; } = new AppUserState();

        public BaseController(CommandFacade commandFacade)
        {
            CommandFacade = commandFacade;
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Grab the user's login information from Identity
            var appUserState = new AppUserState();
            if (User is ClaimsPrincipal user)
            {
                var claims = user.Claims.ToList();

                var userStateString = GetClaim(claims, "userState");
                //var name = GetClaim(claims, ClaimTypes.Name);
                //var id = GetClaim(claims, ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(userStateString))
                    appUserState = new AppUserState(userStateString);
            }
            AppUserState = appUserState;
            ViewData["UserState"] = AppUserState;
        }

        public static string GetClaim(List<Claim> claims, string key)
        {
            return claims.FirstOrDefault(c => c.Type == key)?.Value;
        }

        public async Task<bool> CheckAccessAsync(string action, params string[] resources)
        {
            return await HttpContext.CheckAccessAsync(action, resources);
        }

        public bool CheckAccess(string action, params string[] resources)
        {
            return HttpContext.CheckAccess(action, resources);
        }

        protected CommandFacade CommandFacade { get; }


    }
}