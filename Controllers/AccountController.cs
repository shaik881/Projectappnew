using DefaultIdentityColumnRename.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DefaultIdentityColumnRename.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            Task.Run(() => EnsureRolesExist()).Wait();
        }

        private async Task EnsureRolesExist()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register1.cshtml");
        }

        // Register
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User Not Found");
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Index", "Home");// Or redirect to an appropriate view
        }
    }
}
