using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Models
{
    public class ApplicationRoleViewModel
    {
        public IEnumerable<ExternalSite> ExternalSites { get; set; }

        [Display(Name="Role Name")]
        [Required]
        public string RoleName { get; set; }

        [Display(Name="Description")]
        public string Description { get; set; }

        [Display(Name = "External Sites")]
        public string ExternalSitesCsv { get; set; }
    }

    public class EditApplicationRoleViewModel : ApplicationRoleViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
    }
}