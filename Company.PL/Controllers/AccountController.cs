using Company.DAL.Models.IdentityModels;
using Company.PL.Utilities;
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

        #region Logout
        [HttpGet]
        public IActionResult Logout()
        {
            var result = _signInManager.SignOutAsync();
            if (!result.IsCompletedSuccessfully) TempData["FailMessage"] = "LogOut failed";

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        public IActionResult SendResetPasswordLink(ForgetPasswordViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
                if (user is not null)
                {
                    string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                    string url = Url.Action("ResetPassword", "Account", new {viewModel.Email, Token = token}, "https", "localhost:44301");
                    var email = new Email
                    {
                        To = viewModel.Email,
                        Subject = "CompanyMvc password reset link",
                        Body = $"Click the link below to reset your password\n\n{url}",   // To Do
                    };
                    // Send Email
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Operation");
            return View(nameof(ForgetPassword),viewModel);
        }
        #endregion

        public IActionResult CheckYourInbox() => View();

        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string ?? string.Empty;
                string token = TempData["token"] as string ?? string.Empty;
                
                var user  = _userManager.FindByEmailAsync(email).Result;
                if (user is not null)
                {
                    var result = _userManager.ResetPasswordAsync(user, token, viewModel.Password).Result;

                    if (result.Succeeded)
                        return RedirectToAction(nameof(AccountController.Login));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ModelState.AddModelError("", "Invalid Operation");
            return View(viewModel);
        }
        #endregion
    }
}
