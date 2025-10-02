using AutoMapper;
using Company.BLL.DataTransferObjects.EmployeeDTOs;
using Company.BLL.Services.Interfaces;
using Company.DAL.Models.EmployeeModel;
using Company.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Services.Classes
{
    public class EmployeeService(IEmployeeRepository _employeeRepository, IMapper _mapper) : IEmployeeService
    {
        public IEnumerable<EmployeeDTO> GetAllEmployees(bool isTracked = false)
        {
            return _employeeRepository.GetAll(isTracked).Select(e => _mapper.Map<EmployeeDTO>(e));
            //return _employeeRepository.GetAll(e => _mapper.Map<EmployeeDTO>(e));
        }

        public EmployeeDetailsDTO? GetById(int id)
        {
            var emp = _employeeRepository.GetById(id);

            return emp is null ? null : _mapper.Map<EmployeeDetailsDTO>(emp);
        }

        public int CreateEmployee(CreatedEmployeeDTO createdEmployeeDTO)
        {
            return _employeeRepository.Add(_mapper.Map<Employee>(createdEmployeeDTO));
        }
        public int UpdateEmployee(UpdatedEmployeeDTO updatedEmployeeDTO)
        {
            return _employeeRepository.Update(_mapper.Map<Employee>(updatedEmployeeDTO));
        }

        public bool DeleteEmployee(int id)
        {
            var emp = _employeeRepository.GetById(id);
            if (emp is null) return false;

            emp.IsDeleted = true;
            return _employeeRepository.Update(emp) > 0 ? true : false;
        }

    }
}
