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
    public class EmployeeService(IUnitOfWork _unitOfWork, IMapper _mapper) : IEmployeeService
    {
        public IEnumerable<EmployeeDTO> GetAllEmployees(string? EmployeeSearchName)
        {
            //return _employeeRepository.GetAll(isTracked).Select(e => _mapper.Map<EmployeeDTO>(e));
            //return _employeeRepository.GetAll(e => _mapper.Map<EmployeeDTO>(e));
            IEnumerable<Employee> employees;
            if (string.IsNullOrWhiteSpace(EmployeeSearchName))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.GetAll(e => e.Name.ToLower().Contains(EmployeeSearchName.ToLower()));

            return employees.Select(e => _mapper.Map<EmployeeDTO>(e));
        }

        public EmployeeDetailsDTO? GetById(int id)
        {
            var emp = _unitOfWork.EmployeeRepository.GetById(id);

            return emp is null ? null : _mapper.Map<EmployeeDetailsDTO>(emp);
        }

        public int CreateEmployee(CreatedEmployeeDTO createdEmployeeDTO)
        {
            _unitOfWork.EmployeeRepository.Add(_mapper.Map<Employee>(createdEmployeeDTO));
            return _unitOfWork.SaveChanges();
        }
        public int UpdateEmployee(UpdatedEmployeeDTO updatedEmployeeDTO)
        {
            _unitOfWork.EmployeeRepository.Update(_mapper.Map<Employee>(updatedEmployeeDTO));
            return _unitOfWork.SaveChanges();
        }

        public bool DeleteEmployee(int id)
        {
            var emp = _unitOfWork.EmployeeRepository.GetById(id);
            if (emp is null) return false;

            emp.IsDeleted = true;
            _unitOfWork.EmployeeRepository.Update(emp);
            return _unitOfWork.SaveChanges() > 0 ? true : false;
        }

    }
}
