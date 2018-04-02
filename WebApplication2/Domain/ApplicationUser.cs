using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mtwx.Web.Domain
{
    public class ApplicationUser : DomainObject
    {
        public ApplicationUser()
        {
            ApplicationRoles = new List<ApplicationRole>();
            ExternalSites = new List<ExternalSite>();
        }

        [Required]
        [Display(Name="Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        public ICollection<ApplicationRole> ApplicationRoles { get; }
        public ICollection<ExternalSite> ExternalSites { get; }
    }
}