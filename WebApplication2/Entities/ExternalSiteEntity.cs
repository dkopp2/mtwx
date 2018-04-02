namespace Mtwx.Web.Entities
{
    public class ExternalSiteEntity : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FormId { get; set; }
        public string Href { get; set; }
        public string LoginAction { get; set; }
        public string SiteUserId { get; set; }
        public string SitePassword { get; set; }
        public string FormUserIdField { get; set; }
        public string FormPasswordField { get; set; }
    }
}