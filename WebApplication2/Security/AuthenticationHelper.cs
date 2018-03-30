using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Security
{


    public static class AuthenticationHelper
    {
        public static void IdentitySignin(AppUserState userState, bool isPersistent = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userState.FirstName),
                new Claim("userState", userState.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userState.Email),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", userState.Email)
            };

            claims.AddRange(userState.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // add roles to claims
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = HttpContext.Current.Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;

            authenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7),

            }, identity);
        }

        public static void IdentitySignout()
        {
            var ctx = HttpContext.Current.Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.ExternalCookie);
        }

     }
}