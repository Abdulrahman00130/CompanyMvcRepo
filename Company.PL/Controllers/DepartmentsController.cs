using Company.BLL.DataTransferObjects;
using Company.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class DepartmentsController(IDepartmentService _departmentService,
        ILogger<DepartmentsController> _logger,
        IWebHostEnvironment _environment) : Controller
    {
        public IActionResult Index()
        {
            var depts = _departmentService.GetAllDepartments();
            return View(depts);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreatedDepartmentDTO createdDepartmentDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = _departmentService.AddDepartment(createdDepartmentDTO);
                    if (result > 0)
                        return RedirectToAction("Index");
                    else
                    {
                        ModelState.AddModelError("", "Unable to create the department");
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
            }
            return View(createdDepartmentDTO);
        }

        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            var department = _departmentService.GetDepartmentById(id);
            if (department is null)
            {
                return RedirectToAction("Index");
            }
            return View(department);
        } 
        #endregion
    }
}
