using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mtwx.Web.Commands;
using Mtwx.Web.Controllers;
using Mtwx.Web.Security;
using Thinktecture.IdentityModel.Mvc;
using System.Web;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Areas.Admin.Controllers
{
    public class ExternalSiteController : BaseController
    {
        // GET: ExternalSite
        public async Task<ActionResult> Index()
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.View,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            var externalSites = await CommandFacade.GetExternalSiteList();

            return View(externalSites);
        }

        // GET: ExternalSite/Create
        public async Task<ActionResult> Create()
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.Create,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            return View();
        }

        // POST: ExternalSite/Create
        [HttpPost]
        public async Task<ActionResult> Create(ExternalSite model)
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.Create,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            try
            {
                var userName = User.Identity.Name;

                await CommandFacade.CreateExternalSite(model, userName);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ExternalSite/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.Edit,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            var site = await CommandFacade.GetExternalSite(id);

            return View(site);
        }

        // POST: ExternalSite/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ExternalSite model)
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.Edit,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            try
            {
                var site = await CommandFacade.GetExternalSite(model.Id);

                // update from the model
                site.Description = model.Description;
                site.FormId = model.FormId;
                site.FormPasswordField = model.FormPasswordField;
                site.FormUserIdField = model.FormUserIdField;
                site.Href = model.Href;
                site.LoginAction = model.LoginAction;
                site.SitePassword = model.SitePassword;
                site.SiteUserId = model.SiteUserId;
                site.Name = model.Name;

                await CommandFacade.UpdateExternalSite(site, User.Identity.Name);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ExternalSite/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.Delete,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            var site = await CommandFacade.GetExternalSite(id);

            return View(site);
        }

        // POST: ExternalSite/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            if (!await HttpContext.CheckAccessAsync(
                SecurityResources.Actions.Delete,
                SecurityResources.Modules.AdminModule))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString());
            }

            try
            {
                var result = await CommandFacade.DeleteExternalSite(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ExternalSiteController(CommandFacade commandFacade) : base(commandFacade)
        {
        }
    }
}
