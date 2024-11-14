using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DefaultIdentityColumnRename.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email")]
        [Remote(action: "EmailVerification", controller: "RemoteValidation")]
        public string Email { get; set; }

        [Required, Phone]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
