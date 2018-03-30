using System;
using System.Web.Mvc;

namespace Mtwx.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            ViewBag.StatusCode = statusCode + " Error";

            var fromController = (string)this.RouteData.Values["controller"];
            var fromAction = (string)this.RouteData.Values["action"];

            var info = new HandleErrorInfo(exception, fromController, fromAction);
            return View(info);
        }

        public ActionResult BadRequest()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }
    }
}