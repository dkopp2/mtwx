using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Mtwx.Web.Commands;
using Mtwx.Web.Domain;
using Mtwx.Web.Models;
using Mtwx.Web.Security;

namespace Mtwx.Web.Controllers
{

    public class AccountController : BaseController
    {
        private readonly Func<ILoginProvider> _loginProviderFactory;
        public AccountController(CommandFacade commandFacade, Func<ILoginProvider> loginProviderFactory) : base(commandFacade)
        {
            _loginProviderFactory = loginProviderFactory;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var loginProvider = _loginProviderFactory.Invoke();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await loginProvider.Login(model.Username, model.Password);

            switch (result.ReturnType)
            {
                case LoginReturnTypes.Success:
                    // setup the claims
                    var user = result.ApplicationUser;

                    var userData = new AppUserState(user);

                    AuthenticationHelper.IdentitySignin(userData, false);
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }



        //
        // POST: /Account/LogOff
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }



        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}