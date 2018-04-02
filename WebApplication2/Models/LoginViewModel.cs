using System.ComponentModel.DataAnnotations;

namespace Mtwx.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}