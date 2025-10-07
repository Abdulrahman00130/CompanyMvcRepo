using AutoMapper;
using Company.BLL.DataTransferObjects.EmployeeDTOs;
using Company.BLL.Services.AttachmentService;
using Company.BLL.Services.Interfaces;
using Company.DAL.Models.EmployeeModel;
using Company.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Company.BLL.Services.Classes
{
    public class EmployeeService(IUnitOfWork _unitOfWork, 
        IMapper _mapper,
        IAttachmentService _attachmentService) : IEmployeeService
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
            if (emp is null) return null;

            var empDetailsDTO = _mapper.Map<EmployeeDetailsDTO>(emp);
            return empDetailsDTO;
        }

        public int CreateEmployee(CreatedEmployeeDTO createdEmployeeDTO)
        {
            var employee = _mapper.Map<Employee>(createdEmployeeDTO);
            if(createdEmployeeDTO.Image is not null)
            {
                var imageName = _attachmentService.Upload(createdEmployeeDTO.Image, "Images");
                employee.ImageName = imageName;
            }
            _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.SaveChanges();
        }
        public int UpdateEmployee(UpdatedEmployeeDTO updatedEmployeeDTO)
        {
            var employee = _mapper.Map<Employee>(updatedEmployeeDTO);
            var oldEmp = _unitOfWork.EmployeeRepository.GetById(updatedEmployeeDTO.Id);

            if (updatedEmployeeDTO.Image is not null)
            {
                bool imageDeleted = true;
                if (oldEmp.ImageName is not null)
                {
                    var oldImageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Images", oldEmp.ImageName);
                    imageDeleted = _attachmentService.Delete(oldImageFilePath);
                }
                if(imageDeleted)
                {
                    var imageName = _attachmentService.Upload(updatedEmployeeDTO.Image, "Images");
                    employee.ImageName = imageName;
                }
                
            }
            else
            {
                employee.ImageName = oldEmp.ImageName;
            }
            _unitOfWork.EmployeeRepository.Update(employee);
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
