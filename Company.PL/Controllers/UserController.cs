using Company.DAL.Models.IdentityModels;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class UserController(UserManager<AppUser> _userManager) : Controller
    {
        #region Index
        public IActionResult Index()
        {
            var usersList = _userManager.Users.ToList();

            var users = usersList.Select(u => new UserViewModel
            {
                Id = u.Id,
                FName = u.FirstName,
                LName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Roles = _userManager.GetRolesAsync(u).Result
            }).ToList();

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
    }
}
