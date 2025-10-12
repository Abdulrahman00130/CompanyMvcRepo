using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Company.PL.Controllers
{
    [Authorize]
    public class RoleController(RoleManager<IdentityRole> _roleManager,
                                ILogger<RoleController> _logger,
                                IWebHostEnvironment _environment) : Controller
    {
        #region Index
        public IActionResult Index(string roleSearchName)
        {
            var rolesList = _roleManager.Roles.ToList();
            List<RoleViewModel> roles;

            if (string.IsNullOrWhiteSpace(roleSearchName))
            {
                roles = rolesList.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    RoleName = r.Name
                }).ToList();
            }
            else
            {
                roles = rolesList.Where(r => r.Name.ToLower().Contains(roleSearchName.ToLower()))
                                 .Select(r => new RoleViewModel
                                 {
                                     Id = r.Id,
                                     RoleName = r.Name
                                 }).ToList();
            }

            return View(roles);
        }
        #endregion

        #region Create
        [HttpPost]
        public IActionResult Create(string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(newRoleName)) TempData["FailedCreate"] = "Could not create role";
            else
            {
                var role = new IdentityRole(newRoleName);
                try
                {
                    var result = _roleManager.CreateAsync(role).Result;
                    if (result.Succeeded) TempData["SuccessCreate"] = $"Role {newRoleName} was added successfuly";
                    else
                    {
                        StringBuilder errorMessage = new StringBuilder();
                        foreach (var error in result.Errors)
                        {
                            errorMessage.Append(error.Description);
                            errorMessage.Append("\n");
                        }
                        TempData["FailedCreate"] = errorMessage.ToString();
                    }
                }
                catch (Exception ex)
                {
                    if (_environment.IsDevelopment())
                    {
                        TempData["FailedCreate"] = ex.Message;
                    }
                    else
                    {
                        _logger.LogError(ex.Message);
                    }

                }
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        public IActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var role = _roleManager.FindByIdAsync(id).Result;
            if (role is null) return NotFound();

            var viewModel = new RoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(viewModel);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var role = _roleManager.FindByIdAsync(id).Result;
            if (role is null) return NotFound();

            var viewModel = new RoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] string id, RoleViewModel viewModel)
        {
            if (id is null || id != viewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return View(viewModel);

            var role = _roleManager.FindByIdAsync(id).Result;
            if (role is null) ModelState.AddModelError("", "Invalid Operation");

            role.Name = viewModel.RoleName;
            try
            {
                var result = _roleManager.UpdateAsync(role).Result;
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
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
                }
            }

            return View(viewModel);

        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete([FromRoute] string id)
        {
            if (id is null) return BadRequest();
            var role = _roleManager.FindByIdAsync(id).Result;

            if (role is null) return NotFound();

            var viewModel = new RoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete([FromRoute] string id)
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);

            if (role is not null)
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["SuccessDelete"] = "Role Removed Successfuly";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
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
