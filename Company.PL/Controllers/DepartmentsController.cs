using Company.BLL.DataTransferObjects;
using Company.BLL.DataTransferObjects.DepartmentDTOs;
using Company.BLL.Services.Interfaces;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class DepartmentsController(IDepartmentService _departmentService ,
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
        public IActionResult Create(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdDepartmentDTO = new CreatedDepartmentDTO()
                    {
                        Code = departmentViewModel.Code,
                        Name = departmentViewModel.Name,
                        Description = departmentViewModel.Description,
                        CreateDate = departmentViewModel.DateOfCreation
                    };
                    int result = _departmentService.AddDepartment(createdDepartmentDTO);

                    string message;
                    if (result > 0)
                        message = $"Department {createdDepartmentDTO.Name} was created successfuly!";
                    else
                        message = $"Department {createdDepartmentDTO.Name} was not created :(";
                    TempData["Message"] = message;
                    return RedirectToAction("Index");
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
            return View(departmentViewModel);
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
            return View(new DepartmentViewModel
            {
                Name = dept.Name,
                Code = dept.Code,
                Description = dept.Description,
                DateOfCreation = dept.CreateDate
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel model)
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

        [Authorize(Roles = "Admin")]
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
