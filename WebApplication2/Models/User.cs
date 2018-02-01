using System.ComponentModel.DataAnnotations;

namespace Mtwx.Web.Models
{
    public class User
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool IsValid(string _username, string _password) => true;
    }
}