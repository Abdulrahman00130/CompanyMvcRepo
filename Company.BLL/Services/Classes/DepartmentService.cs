using Company.BLL.DataTransferObjects.DepartmentDTOs;
using Company.BLL.Factories;
using Company.BLL.Services.Interfaces;
using Company.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Services.Classes
{
    public class DepartmentService(IUnitOfWork _unitOfWork) : IDepartmentService
    {
        public IEnumerable<AllDepartmentsDTO> GetAllDepartments() =>
             _unitOfWork.DepartmentRepository.GetAll().Select(d => d.ToAllDepartmentsDTO());

        public DepartmentByIdDTO? GetDepartmentById(int id) =>
            _unitOfWork.DepartmentRepository.GetById(id).ToDepartmentByIdDTO();

        public int AddDepartment(CreatedDepartmentDTO createdDepartment)
        {
            _unitOfWork.DepartmentRepository.Add(createdDepartment.ToEntity());
            return _unitOfWork.SaveChanges();

        }
        public int UpdateDepartment(UpdatedDepartmentDTO updatedDepartment)
        {
            _unitOfWork.DepartmentRepository.Update(updatedDepartment.ToEntity());
            return _unitOfWork.SaveChanges();
        }
        public bool RemoveDepartment(int id)
        {
            var dept = _unitOfWork.DepartmentRepository.GetById(id);
            if (dept is null) return false;

            _unitOfWork.DepartmentRepository.Remove(dept);
            return _unitOfWork.SaveChanges() > 0 ? true : false;
        }
    }
}
