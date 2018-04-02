using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mtwx.Web.Commands;
using Mtwx.Web.Controllers;
using Mtwx.Web.Domain;
using Mtwx.Web.Models;

namespace Mtwx.Web.Areas.Admin.Controllers
{
    public class ApplicationRoleController : BaseController
    {
        // GET: Admin/ApplicationRole
        public async Task<ActionResult> Index()
        {
            var roles = await CommandFacade.GetApplicationRoleList();
            return View(roles);
        }

        // GET: Admin/ApplicationRole/Create
        public async Task<ActionResult> Create()
        {
            var sites = await CommandFacade.GetExternalSiteList();

            var model = new ApplicationRoleViewModel()
            {
                ExternalSites = sites
            };

            return View(model);
        }

        // POST: Admin/ApplicationRole/Create
        [HttpPost]
        public async Task<ActionResult> Create(ApplicationRoleViewModel model)
        {
                var sites = await CommandFacade.GetExternalSiteList();
            try
            {

                var userName = User.Identity.Name;

                var role = new ApplicationRole()
                {
                    Description = model.Description,
                    RoleName = model.RoleName
                };

                if (!string.IsNullOrEmpty(model.ExternalSitesCsv))
                {
                    var selectedSites = model.ExternalSitesCsv.Split(',');
                    foreach (var site in selectedSites)
                    {
                        role.ExternalSites.Add(new ExternalSite() { Id = int.Parse(site) });
                    }
                }

                await CommandFacade.CreateApplicationRole(role, userName);

                return RedirectToAction("Index");
            }
            catch
            {
                model.ExternalSites = sites;
                return View(model);
            }
        }

        // GET: Admin/ApplicationRole/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var sites = await CommandFacade.GetExternalSiteList();

            var role = await CommandFacade.GetApplicationRole(id);

            var model = new EditApplicationRoleViewModel()
            {
                ExternalSites = sites,
                Id = role.Id,
                Description = role.Description,
                RoleName = role.RoleName,
                ExternalSitesCsv = string.Join(",", role.ExternalSites.Select(x => x.Id.ToString()))
            };

            return View(model);
        }

        // POST: Admin/ApplicationRole/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditApplicationRoleViewModel model)
        {
            var sites = await CommandFacade.GetExternalSiteList();
            try
            {
                // get the current application role
                var role = await CommandFacade.GetApplicationRole(model.Id);

                // update the fields
                role.Description = model.Description;
                role.RoleName = model.RoleName;
                role.ExternalSites.Clear();
                if (!string.IsNullOrEmpty(model.ExternalSitesCsv))
                {
                    var selectedSites = model.ExternalSitesCsv.Split(',');
                    foreach (var site in selectedSites)
                    {
                        role.ExternalSites.Add(new ExternalSite() { Id = int.Parse(site) });
                    }
                }

                // save the role
                var userName = User.Identity.Name;
                var result = await CommandFacade.UpdateApplicationRole(role, userName);

                return RedirectToAction("Index");
            }
            catch
            {
                model.ExternalSites = sites;
                return View(model);
            }
        }

        // GET: Admin/ApplicationRole/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var role = await CommandFacade.GetApplicationRole(id);
            return View(role);
        }

        // POST: Admin/ApplicationRole/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            try
            {
                var result = await CommandFacade.DeleteApplicationRole(id);

                return RedirectToAction("Index");
            }
            catch
            {
                var role = CommandFacade.GetApplicationRole(id);
                return View(role);
            }
        }

        public ApplicationRoleController(CommandFacade commandFacade) : base(commandFacade)
        {
        }
    }
}
