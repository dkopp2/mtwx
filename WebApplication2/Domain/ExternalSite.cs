using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mtwx.Web.Domain
{
    public class ExternalSite : DomainObject
    {
        [Display(Name="Site Name")]
        public string Name { get; set; }
        [Display(Name="Site Description")]
        public string Description { get; set; }
        [Display(Name = "Form Id")]
        public string FormId { get; set; }
        [Display(Name = "HREF")]
        public string Href { get; set; }
        [Display(Name = "Login Action URL")]
        public string LoginAction { get; set; }
        [Display(Name = "Site User ID")]
        public string SiteUserId { get; set; }
        [Display(Name = "Site Password")]
        public string SitePassword { get; set; }
        [Display(Name = "Form User ID Field ID")]
        public string FormUserIdField { get; set; }
        [Display(Name = "Form Password Field ID")]
        public string FormPasswordField { get; set; }
    }
}