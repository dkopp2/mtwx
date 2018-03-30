using System;
using System.Collections.Generic;

namespace Mtwx.Web.Domain
{
    public class AppUserState
    {
        public AppUserState()
        {
            Roles = new List<string>();
        }

        public AppUserState(ApplicationUser user) : this()
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;

            foreach (var r in user.ApplicationRoles)
            {
                Roles.Add(r.RoleName);
            }
        }

        public AppUserState(string itemString) : this()
        {

            if (string.IsNullOrEmpty(itemString))
            {
                return;
            }

            var strings = itemString.Split('~');
            if (strings.Length < 2)
                throw new ArgumentOutOfRangeException();

            var valsString = strings[0];
            var rolesString = strings[1];

            var vals = valsString.Split('|');
            var roles = rolesString.Split('|');

            FirstName = vals[0];
            LastName = vals[1];
            Email = vals[2];

            Roles.Clear();
            foreach (var r in roles)
            {
                Roles.Add(r);
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<string> Roles { get; }

        /// <summary>
        /// Exports a short string list of Id, Email, Name separated by |
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var roles = string.Join("|", Roles);
            var vals = string.Join("|", FirstName, LastName, Email);
            return $"{vals}~{roles}";
        }
    }
}