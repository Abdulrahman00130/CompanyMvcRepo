using Company.BLL.DataTransferObjects.EmployeeDTOs;
using Company.BLL.Services.Interfaces;
using Company.DAL.Models.Employee;
using Company.DAL.Models.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class EmployeesController(IEmployeeService _employeeService,
                                     ILogger<EmployeesController> _logger,
                                     IWebHostEnvironment _environment) : Controller
    {
        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
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
        public IActionResult Create(CreatedEmployeeDTO employeeDTO)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    int result = _employeeService.CreateEmployee(employeeDTO);
                    if (result > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to create employee");
                    }
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
            return View(employeeDTO);
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
            return View(new UpdatedEmployeeDTO
            {
                Id = id.Value,
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HiringDate = employee.HiringDate,
                Salary = employee.Salary,
                Gender = Enum.Parse<Gender>(employee.Gender),
                EmployeeType = Enum.Parse<EmployeeType>(employee.EmployeeType)
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute]int? id,UpdatedEmployeeDTO employeeDTO)
        {
            if (!id.HasValue || id.Value != employeeDTO.Id) return BadRequest();
            if (!ModelState.IsValid) return View (employeeDTO);
            try
            {
                int result = _employeeService.UpdateEmployee(employeeDTO);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to Update employee");
                    return View(employeeDTO);
                }
            }
            catch (Exception ex)
            {
                if (_environment.IsDevelopment())
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(employeeDTO);
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
