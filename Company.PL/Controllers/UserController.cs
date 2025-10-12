using Company.DAL.Models.IdentityModels;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Authorize]
    public class UserController(UserManager<AppUser> _userManager, 
                                ILogger<UserController> _logger, 
                                IWebHostEnvironment _environment) : Controller
    {
        #region Index
        public IActionResult Index(string userSearchName)
        {
            var usersList = _userManager.Users.ToList();
            List<UserViewModel> users;

            if(string.IsNullOrWhiteSpace(userSearchName))
            {
                users = usersList.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FName = u.FirstName,
                    LName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).ToList();
            }
            else
            {
                users = usersList.Where(u => $"{u.FirstName}{u.LastName}".ToLower().Contains(userSearchName.ToLower()))
                                 .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FName = u.FirstName,
                    LName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).ToList();
            }

            return View(users);
        }
        #endregion

        #region Details
        public IActionResult Details(string id)
        {
            if (id is null) return BadRequest();
            var user = _userManager.FindByIdAsync(id).Result;
            if (user is null) return NotFound();
            var userDetails = new UserDetailsViewModel
            {
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View (userDetails);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id is null) return BadRequest();
            var user = _userManager.FindByIdAsync(id).Result;

            if (user is null) return NotFound();

            //TempData["userName"] = user.UserName;
            var userEdit = new UserEditViewModel
            {
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
            return View(userEdit);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] string id, UserEditViewModel viewModel)
        {
            if (id is null || id != viewModel.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = _userManager.FindByIdAsync(id).Result;
                    if (oldUser is not null)
                    {
                        oldUser.FirstName = viewModel.FName;
                        oldUser.LastName = viewModel.LName;
                        oldUser.PhoneNumber = viewModel.PhoneNumber;

                        var result = _userManager.UpdateAsync(oldUser).Result;
                        if (result.Succeeded) return RedirectToAction(nameof(Index));

                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                    else
                    {
                        _logger.LogError(ex.Message);
                        return View("ErrorView", ex);
                    }
                }

            }
            //TempData.Keep("userName");
            ModelState.AddModelError("", "Invalid Edit Operation");
            return View(viewModel);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete([FromRoute] string id)
        {
            if (id is null) return BadRequest();
            var user = _userManager.FindByIdAsync(id).Result;

            if (user is null) return NotFound();

            var viewModel = new UserDetailsViewModel
            {
                Id = user.Id,
                FName = user.FirstName,
                LName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete([FromRoute]string id)
        {
            if (id is null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);

            if(user is not null)
            {
                try
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["SuccessDelete"] = "User Removed Successfuly";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        TempData["FailedDelete"] = ex.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError(ex.Message);
                        return View("ErrorView", ex);
                    }
                }
            }

            TempData["FailedDelete"] = "Invalid Operation";
            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
