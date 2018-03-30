using System.Collections.Generic;
using System.ComponentModel;

namespace Mtwx.Web.Domain
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            ApplicationRoles = new List<ApplicationRole>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<ApplicationRole> ApplicationRoles { get; }
    }
}