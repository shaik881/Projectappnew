using DefaultIdentityColumnRename.Data;
using DefaultIdentityColumnRename.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DefaultIdentityColumnRename.Controllers
{
    public class RemoteValidationController : Controller
    {
        private readonly UserManager<User> userManager;
        public RemoteValidationController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> LoginEmail(string email)
        {
            if (email == null)
            {
                return Json("Email Cannot be Null");
            }
            var mail = await userManager.FindByEmailAsync(email);
            if (mail == null)
            {
                return Json($"{email} does not exists");
            }
            return Json(true);
        }
        public async Task<IActionResult> EmailVerification(string email)
        {
            if(email == null)
            {
                ModelState.AddModelError("","Email Cannot be null");
                return Json(false);
            }
            var mail = await userManager.FindByEmailAsync(email);
            if(mail != null)
            {
                return Json($"{email} is already taken");
            }
            return Json(true);
        }

    }
}
