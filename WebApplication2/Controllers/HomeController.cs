using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mtwx.Web.Commands;
using Mtwx.Web.Domain;
using Mtwx.Web.Models;
using Mtwx.Web.Security;

namespace Mtwx.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult SiteMenu()
        {
            var userEmail = AuthenticationHelper.GetClaimValue(ClaimTypes.NameIdentifier);
            var sites = AsyncHelpers.RunSync(() => CommandFacade.GetCurrentUserSiteList(userEmail));
            return PartialView("_SiteMenu", sites);
        }

        public HomeController(CommandFacade commandFacade) : base(commandFacade)
        {
        }
    }
}