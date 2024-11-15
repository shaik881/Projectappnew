using DefaultIdentityColumnRename.Models;
using DefaultIdentityColumnRename.Services;
using DefaultIdentityColumnRename.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DefaultIdentityColumnRename.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProfileService _profileService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,ProfileService profileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _profileService = profileService;
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
                    UserProfile profile = new UserProfile()
                    {
                        UserId = user.Id
                    };
                    _profileService.Login(profile);
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
            return View("~/Views/Account/Login1.cshtml");
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
            return View("~/Views/Account/Login1.cshtml");
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
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var UserProfile = _profileService.FindUserProfile(user);
            if (user == null )
                return BadRequest("user");
            ProfileViewModel profile = new ProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                Address = UserProfile?.Address,
                PinCode = UserProfile.Pincode,
                State = UserProfile?.State,
                Gender = UserProfile?.Gender,
                ShowPic = UserProfile?.Pic,
                Country = UserProfile?.Country
            };
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("user is null");
            }
            user.MobileNumber = model.MobileNumber;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var profile = new UserProfile()
            {
                Pic = await _profileService.ConvertToByteArray(model.Pic),
                Address = model.Address,
                Pincode = model.PinCode,
                State = model.State,
                Gender = model.Gender,
                Country = model.Country,
                UserId = user.Id,
            };
           var result =  await _userManager.UpdateAsync(user);
           var res = await _profileService.UpdateProfile(profile);
            if(result.Succeeded&&res)
            {
                return Ok("profile updated Succesfully");
            }
            return View(model);
        }
    }
}
