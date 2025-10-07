using Company.BLL.DataTransferObjects.EmployeeDTOs;
using Company.BLL.Services.Interfaces;
using Company.DAL.Models.EmployeeModel;
using Company.DAL.Models.Shared.Enums;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeesController(IEmployeeService _employeeService,
                                     ILogger<EmployeesController> _logger,
                                     IWebHostEnvironment _environment) : Controller
    {
        public IActionResult Index(string? EmployeeSearchName)
        {
            var employees = _employeeService.GetAllEmployees(EmployeeSearchName);
            return View(employees);
        }

        #region Create
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    int result = _employeeService.CreateEmployee(new CreatedEmployeeDTO()
                    {
                        Name = employeeViewModel.Name,
                        Address = employeeViewModel.Address,
                        Age = employeeViewModel.Age,
                        IsActive = employeeViewModel.IsActive,
                        Email = employeeViewModel.Email,
                        EmployeeType = employeeViewModel.EmployeeType,
                        Gender = employeeViewModel.Gender,
                        HiringDate = employeeViewModel.HiringDate,
                        PhoneNumber = employeeViewModel.PhoneNumber,
                        Salary = employeeViewModel.Salary,
                        DepartmentId = employeeViewModel.DepartmentId,
                        Image = employeeViewModel.Image,
                    });

                    string message;
                    if (result > 0)
                        message = $"Employee {employeeViewModel.Name} was added successfuly";
                    else
                        message = $"Employee {employeeViewModel.Name} was not added";

                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    if(_environment.IsDevelopment())
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                    else
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
            return View(employeeViewModel);
        }
        #endregion

        #region Details
        public IActionResult Details(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetById(id.Value);
            if (employee is null) return NotFound();
            return View(employee);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var employee = _employeeService.GetById(id.Value);
            if(employee is null) return NotFound();
            return View(new EmployeeViewModel
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                Salary = employee.Salary,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType),
                DepartmentId = employee.DepartmentId,
                ImageName = employee.ImageName,
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int? id, EmployeeViewModel employeeViewModel)
        {
            if (!id.HasValue) return BadRequest();
            if (!ModelState.IsValid) return View (employeeViewModel);
            try
            {
                int result = _employeeService.UpdateEmployee(new UpdatedEmployeeDTO()
                {
                    Id = id.Value,
                    Name = employeeViewModel.Name,
                    Address = employeeViewModel.Address,
                    Age= employeeViewModel.Age,
                    IsActive = employeeViewModel.IsActive,
                    Email = employeeViewModel.Email,
                    EmployeeType = employeeViewModel.EmployeeType,
                    Gender = employeeViewModel.Gender,
                    HiringDate= employeeViewModel.HiringDate,
                    PhoneNumber = employeeViewModel.PhoneNumber,
                    Salary = employeeViewModel.Salary,
                    DepartmentId = employeeViewModel.DepartmentId,
                    Image = employeeViewModel.Image,
                });
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to Update employee");
                    return View(employeeViewModel);
                }
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(employeeViewModel);
                }
                else
                {
                    _logger.LogError(ex.Message);
                    return View("ErrorView",ex);
                }
            }
            
        }
        #endregion

        #region Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0) return BadRequest();
            try
            {
                bool isdeleted = _employeeService.DeleteEmployee(id);
                if (isdeleted) return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to delete the employee");
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
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
