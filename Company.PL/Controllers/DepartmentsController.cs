using Company.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class DepartmentsController(IDepartmentService _departmentService) : Controller
    {
        public IActionResult Index()
        {
            var depts = _departmentService.GetAllDepartments();
            return View(depts);
        }
    }
}
