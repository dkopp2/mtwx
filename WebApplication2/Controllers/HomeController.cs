using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mtwx.Web.Commands;
using Mtwx.Web.Domain;
using Mtwx.Web.Models;

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
            var sites = AsyncHelpers.RunSync(() => CommandFacade.GetExternalSiteList());
            return PartialView("_SiteMenu", sites);
        }

        public HomeController(CommandFacade commandFacade) : base(commandFacade)
        {
        }
    }
}