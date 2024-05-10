using Phenicienn.CustomValidationAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Phenicienn.Models.User
{
    public class RegisterUser
    {

        [Required]
        [DisplayName("Username")]
        [MinLength(8, ErrorMessage = "Username should be at least 8 characters")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Password should be at least 8 characters")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string confirm_password { get; set; }
    }
}
