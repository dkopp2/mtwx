using System.Collections.Generic;

namespace Mtwx.Web.Domain
{
    public class ApplicationRole : DomainObject
    {
        public ApplicationRole()
        {
            ExternalSites = new List<ExternalSite>();
        }

        public string RoleName { get; set; }
        public string Description { get; set; }

        public ICollection<ExternalSite> ExternalSites { get; }
        /// <summary>
        /// Exports a short string list of Id, Email, Name separated by |
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join("|", new string[] { Id.ToString(), RoleName, Description });
        }

        /// <summary>
        /// Imports Id, Email and Name from a | separated string
        /// </summary>
        /// <param name="itemString"></param>
        public bool FromString(string itemString)
        {
            if (string.IsNullOrEmpty(itemString))
                return false;

            var strings = itemString.Split('|');
            if (strings.Length < 3)
                return false;

            Id = int.Parse(strings[0]);
            RoleName = strings[1];
            Description = strings[2];

            return true;
        }

        /// <summary>
        /// Populates the AppUserState properties from a
        /// User instance
        /// </summary>
        /// <param name="role"></param>
        public void FromUser(ApplicationRole role)
        {
            Id = role.Id;
            RoleName = role.RoleName;
            Description = role.Description;
        }
    }
}