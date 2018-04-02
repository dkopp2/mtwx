using System.Web.Mvc;
using System.Web.Routing;

namespace Mtwx.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "ExternalSite", action = "Index", id = UrlParameter.Optional }
            ).DataTokens = new RouteValueDictionary(new { area = "Admin" });
        }
    }
}