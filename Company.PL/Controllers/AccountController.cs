using Company.DAL.Models.IdentityModels;
using Company.PL.Utilities;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Security.Policy;

namespace Company.PL.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, 
                                   SignInManager<AppUser> _signInManager,
                                   IEmailService _emailService,
                                   ITwilioService _twilioService) : Controller
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

        #region LoginUsingGoogle
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        //    {
        //        claim.Issuer,
        //        claim.OriginalIssuer,
        //        claim.Type,
        //        claim.Value,
        //    });

        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}

        public async Task<IActionResult> GoogleResponse()
        {

            // Try to get the external login info (standard way)
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // Fallback: try to authenticate directly with the Google scheme
                var authResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                if (!authResult.Succeeded || authResult.Principal == null)
                    return RedirectToAction(nameof(Login));

                info = new ExternalLoginInfo(authResult.Principal,
                                             GoogleDefaults.AuthenticationScheme,
                                             authResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                                             GoogleDefaults.AuthenticationScheme);
            }

            // 1) If an existing local user is already linked to this external login -> sign in
            var externalSignIn = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (externalSignIn.Succeeded)
            {
                // Clear the temporary external cookie
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // 2) Not linked yet. Get email from the external provider
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var givenName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var familyName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            if (string.IsNullOrEmpty(email))
            {
                // We require email to create an account. Show message and redirect to login.
                TempData["FailMessage"] = "No email address provided. Please register manually.";
                return RedirectToAction(nameof(Login));
            }

            // 3) If a local user with this email exists -> link and sign in
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                {
                    TempData["FailMessage"] = "Failed to link Google account: " + string.Join("; ", addLoginResult.Errors.Select(e => e.Description));
                    return RedirectToAction(nameof(Login));
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // 4) No local user -> create one, link external login, and sign in (auto-create)
            user = new AppUser
            {
                UserName = email,
                Email = email,
                FirstName = givenName ?? name ?? string.Empty,
                LastName = familyName ?? string.Empty,
                EmailConfirmed = true // mark email confirmed since it came from Google
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                TempData["FailMessage"] = "Failed to create local user: " + string.Join("; ", createResult.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Login));
            }

            var linkResult = await _userManager.AddLoginAsync(user, info);
            if (!linkResult.Succeeded)
            {
                // If linking fails, optionally delete created user to avoid orphan records.
                await _userManager.DeleteAsync(user);
                TempData["FailMessage"] = "Failed to link Google login to the new user: " + string.Join("; ", linkResult.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Login));
            }

            // Finalize: sign the user in and clear external cookie
            await _signInManager.SignInAsync(user, isPersistent: false);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #endregion

        #region Logout
        [HttpGet]
        public IActionResult Logout()
        {
            var result = _signInManager.SignOutAsync();
            if (!result.IsCompletedSuccessfully) TempData["FailMessage"] = "LogOut failed";

            return RedirectToAction(nameof(AccountController.Login), "Account");
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
                        Body = $"Click the link below to reset your password\n\n{url}",
                    };

                    // Send Email
                    try
                    {
                        _emailService.SendEmail(email);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        return View(nameof(ForgetPassword), viewModel);
                    }
                    return RedirectToAction(nameof(AccountController.CheckYourInbox));
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Operation");
            return View(nameof(ForgetPassword),viewModel);
        }

        #region SendResetPasswordLinkSMS
        [HttpPost]
        public IActionResult SendResetPasswordLinkSMS(ForgetPasswordViewModel viewModel)
        {
            if(!ModelState.IsValid) return View(nameof(ForgetPassword), viewModel);

            var user = _userManager.FindByEmailAsync(viewModel.Email).Result;
            if (user is not null)
            {
                string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                string url = Url.Action(nameof(ResetPassword), "Account", new { user.Email, Token = token }, "https", "localhost:44301");
                var sms = new SMS
                {
                    To = user.PhoneNumber,
                    Body = $"Click the link below to reset your password\n\n{url}"
                };

                //send SMS
                try
                {
                    _twilioService.SendSMS(sms);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(nameof(ForgetPassword), viewModel);
                }
                return RedirectToAction(nameof(CheckPhoneMessages));
            }

            ModelState.AddModelError("", "Invalid Operation");
            return View(nameof(ForgetPassword), viewModel);
        }
        #endregion

        #endregion

        public IActionResult CheckYourInbox() => View();
        public IActionResult CheckPhoneMessages() => View();
        public IActionResult AccessDenied() => View();

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
            string email = TempData["email"] as string ?? string.Empty;
            string token = TempData["token"] as string ?? string.Empty;
            if (ModelState.IsValid)
            {
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

            TempData.Keep("email");
            TempData.Keep("token");
            ModelState.AddModelError("", "Invalid Operation");
            return View(viewModel);
        }
        #endregion
    }
}
