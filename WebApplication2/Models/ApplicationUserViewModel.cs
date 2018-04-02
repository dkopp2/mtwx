using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mtwx.Web.Domain;

namespace Mtwx.Web.Models
{
    public class ApplicationUserViewModel
    {
        public IEnumerable<ApplicationRole> ApplicationRoles { get; set; }
        public IEnumerable<ExternalSite> ExternalSites { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Roles")]
        public string ApplicationRolesCsv { get; set; }

        [Display(Name = "External Sites")]
        public string ExternalSitesCsv { get; set; }

    }

    public class EditApplicationUserViewModel : ApplicationUserViewModel
    {
        public int Id { get; set; }
    }
}