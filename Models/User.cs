using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DefaultIdentityColumnRename.Models
{
    public class User:IdentityUser
    {
        //git changes
        [Required]
        public string FirstName {  get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public override string Email { get; set; }

        [Required, Phone]
        public string MobileNumber { get; set; }
        public UserProfile Profile { get; set; }
    }
}

