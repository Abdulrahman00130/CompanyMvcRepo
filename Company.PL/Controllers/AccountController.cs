using Company.DAL.Models.IdentityModels;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Company.PL.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : Controller
    {

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if(!ModelState.IsValid) return View(viewModel);

            var user = new AppUser 
            { 
                UserName = viewModel.UserName,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
            };
            var result = _userManager.CreateAsync(user, viewModel.Password).Result;
            if (result.Succeeded) return RedirectToAction("Login");
            else
            {
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(viewModel);
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
            if (user is not null)
            {
                bool flag = _userManager.CheckPasswordAsync(user, viewModel.Password).Result;
                if (flag)
                {
                    var result = _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false).Result;
                    if (result.IsNotAllowed)
                        ModelState.AddModelError(string.Empty, "Your Account is Not Allowed");
                    if (result.IsLockedOut)
                        ModelState.AddModelError(string.Empty, "Your Account is Locked Out");
                    if (result.Succeeded)
                        return RedirectToAction(nameof(HomeController.Index), "Home");

                    return View(viewModel);
                }

            }

            ModelState.AddModelError(string.Empty, "Invalid Login");
            return View(viewModel);

        }

        #endregion
    }
}
