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
    public class ApplicationUserController : BaseController
    {
        // GET: Admin/ApplicationUser
        public async Task<ActionResult> Index()
        {
            var users = await CommandFacade.GetApplicationUserList();
            return View(users);
        }

        // GET: Admin/ApplicationUser/Create
        public async Task<ActionResult> Create()
        {
            var roles = await CommandFacade.GetApplicationRoleList();
            var sites = await CommandFacade.GetExternalSiteList();

            var model = new ApplicationUserViewModel()
            {
                ApplicationRoles = roles,
                ExternalSites = sites
            };

            return View(model);
        }

        // POST: Admin/ApplicationUser/Create
        [HttpPost]
        public async Task<ActionResult> Create(ApplicationUserViewModel model)
        {
            var roles = await CommandFacade.GetApplicationRoleList();
            var sites = await CommandFacade.GetExternalSiteList();

            try
            {
                var userName = User.Identity.Name;
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password == null ? string.Empty : model.Password.Protect()
                };

                if (!string.IsNullOrEmpty(model.ApplicationRolesCsv))
                {
                    var userRoles = model.ApplicationRolesCsv.Split(',');
                    foreach (var role in userRoles)
                    {
                        user.ApplicationRoles.Add(new ApplicationRole() { Id = int.Parse(role) });
                    }
                }

                if (!string.IsNullOrEmpty(model.ExternalSitesCsv))
                {
                    var userSites = model.ExternalSitesCsv.Split(',');
                    foreach (var site in userSites)
                    {
                        user.ExternalSites.Add(new ExternalSite() { Id = int.Parse(site) });
                    }
                }

                await CommandFacade.CreateApplicationUser(user, userName);

                return RedirectToAction("Index");
            }
            catch
            {
                model.ApplicationRoles = roles;
                model.ExternalSites = sites;
                return View(model);
            }
        }

        // GET: Admin/ApplicationUser/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var roles = await CommandFacade.GetApplicationRoleList();
            var sites = await CommandFacade.GetExternalSiteList();

            var user = await CommandFacade.GetApplicationUser(id);

            var model = new EditApplicationUserViewModel()
            {
                ApplicationRoles = roles,
                ExternalSites = sites,
                Id = user.Id,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Password = user.Password == null ? string.Empty : user.Password.TryUnprotect(defaultValue:user.Password),
                Email = user.Email,
                ExternalSitesCsv = string.Join(",", user.ExternalSites.Select(x => x.Id.ToString())),
                ApplicationRolesCsv = string.Join(",", user.ApplicationRoles.Select(x => x.Id.ToString()))
            };

            return View(model);
        }

        // POST: Admin/ApplicationUser/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EditApplicationUserViewModel model)
        {
            var allRoles = await CommandFacade.GetApplicationRoleList();
            var allSites = await CommandFacade.GetExternalSiteList();

            try
            {
                var userName = User.Identity.Name;
                // update the user
                var user = await CommandFacade.GetApplicationUser(model.Id);

                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Password = model.Password == null ? string.Empty : model.Password.Protect();

                user.ApplicationRoles.Clear();
                if (!string.IsNullOrEmpty(model.ApplicationRolesCsv))
                {
                    var roles = model.ApplicationRolesCsv.Split(',');
                    foreach (var role in roles)
                    {
                        user.ApplicationRoles.Add(new ApplicationRole() { Id = int.Parse(role) });
                    }
                }

                user.ExternalSites.Clear();
                if (!string.IsNullOrEmpty(model.ExternalSitesCsv))
                {
                    var sites = model.ExternalSitesCsv.Split(',');
                    foreach (var site in sites)
                    {
                        user.ExternalSites.Add(new ExternalSite() { Id = int.Parse(site) });
                    }
                }

                // save the user information
                await CommandFacade.UpdateApplicationUser(user, userName);

                return RedirectToAction("Index");
            }
            catch
            {
                model.ExternalSites = allSites;
                model.ApplicationRoles = allRoles;

                return View(model);
            }
        }

        // GET: Admin/ApplicationUser/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await CommandFacade.GetApplicationUser(id);
            return View(user);
        }

        // POST: Admin/ApplicationUser/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(ApplicationUser user)
        {
            try
            {
                await CommandFacade.DeleteApplicationUser(user.Id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(user);
            }
        }

        public ApplicationUserController(CommandFacade commandFacade) : base(commandFacade)
        {
        }
    }
}
