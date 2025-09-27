using Company.BLL.DataTransferObjects;
using Company.BLL.Services;
using Company.PL.ViewModels;
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
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();  //400
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null) return NotFound();  //404
            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var dept = _departmentService.GetDepartmentById(id.Value);
            if (dept is null) return NotFound();
            return View(new DepartmentEditViewModel
            {
                Name = dept.Name,
                Code = dept.Code,
                Description = dept.Description,
                DateOfCreation = dept.CreateDate
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, DepartmentEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var result = _departmentService.UpdateDepartment(new UpdatedDepartmentDTO
                    {
                        Id = id,
                        Name = model.Name,
                        Code = model.Code,
                        Description = model.Description,
                        CreateDate = model.DateOfCreation
                    });
                    if (result > 0) return RedirectToAction("Index");
                    ModelState.AddModelError("", "Unable to edit the department");

                }
                catch (Exception ex)
                {
                    if(_environment.IsDevelopment())
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
                return View(model);
        }
        #endregion

        #region Delete
        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (!id.HasValue) return BadRequest();
        //    var dept = _departmentService.GetDepartmentById(id.Value);
        //    if (dept is null) return NotFound();
        //    return View(dept);
        //}

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool deleted = _departmentService.RemoveDepartment(id);
                if (deleted) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Unable to delete the department");
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    ModelState.AddModelError("", ex.Message);
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _logger.LogError(ex.Message);
                    return View("ErrorView", ex);
                }
            }
        }
        #endregion

    }
}
