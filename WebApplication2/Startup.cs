using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Mtwx.Web.Configuration.Ioc;
using Mtwx.Web.Security;
using Owin;
using Serilog;

[assembly: OwinStartup(typeof(Mtwx.Web.Startup))]
namespace Mtwx.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ContainerFactory.DefaultContainer;
            var logger = container.Resolve<ILogger>();

            // Register the Autofac middleware FIRST. This also adds
            // Autofac-injected middleware registered with the container.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app, logger);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Information("Application Start");
        }

        public void ConfigureAuth(IAppBuilder app, ILogger logger)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(120),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieManager = new SystemWebCookieManager(),
                CookieSecure = CookieSecureOption.SameAsRequest,
                CookieName = ".Mtwx.Web"
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            // configure resource authorization
            app.UseResourceAuthorization(new AuthorizationManager());
        }

    }
}