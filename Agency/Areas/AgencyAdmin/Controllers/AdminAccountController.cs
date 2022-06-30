using Agency.Areas.AgencyAdmin.ViewModels;
using Agency.Models;
using Agency.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agency.Areas.AgencyAdmin.Controllers
{
    [Area("AgencyAdmin")]
    public class AdminAccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminAccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                UserName = register.Username,
                Email = register.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM user)
        {
            AppUser existedUser = await _userManager.FindByNameAsync(user.Username);
            IList<string> roles = await _userManager.GetRolesAsync(existedUser);
            string adminRole = roles.FirstOrDefault(r => r.ToLower().Trim() == Roles.Admin.ToString().ToLower().Trim());
            string superAdminRole = roles.FirstOrDefault(r => r.ToLower().Trim() == Roles.SuperAdmin.ToString().ToLower().Trim());
            if (adminRole == null && superAdminRole == null)
            {
                ModelState.AddModelError("", "Something went wrong.Please try again");
                return View();
            }
            else
            {
                if (user.RememberMe)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(existedUser, user.Password, true, true);
                    if (!result.Succeeded)
                    {
                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError("", "You have been dismissed for 5 minutes");
                            return View();
                        }
                        ModelState.AddModelError("", "Username or password is incorrect");
                        return View();
                    }
                }
                else
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(existedUser, user.Password, false, true);
                    if (!result.Succeeded)
                    {
                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError("", "You have been dismissed for 5 minutes");
                            return View();
                        }
                        ModelState.AddModelError("", "Username or password is incorrect");
                        return View();
                    }
                }
                return RedirectToAction("Index", "Dashboard");
            }
            
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Show()
        {
            return Content(User.Identity.IsAuthenticated.ToString());
        }
        public async Task CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = Roles.SuperAdmin.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
        }
    }
}
