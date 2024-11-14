using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DefaultIdentityColumnRename.Models
{
    public class LoginViewModel
    {
       // [Required, EmailAddress]
        //[Remote(action: "LoginEmail", controller: "RemoteValidation")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
