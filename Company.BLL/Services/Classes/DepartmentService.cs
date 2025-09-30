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
    public class DepartmentService(IDepartmentRepository _departmentRepository) : IDepartmentService
    {
        public IEnumerable<AllDepartmentsDTO> GetAllDepartments() =>
             _departmentRepository.GetAll().Select(d => d.ToAllDepartmentsDTO());

        public DepartmentByIdDTO? GetDepartmentById(int id) =>
            _departmentRepository.GetById(id).ToDepartmentByIdDTO();

        public int AddDepartment(CreatedDepartmentDTO createdDepartment) =>
            _departmentRepository.Add(createdDepartment.ToEntity());
        public int UpdateDepartment(UpdatedDepartmentDTO updatedDepartment) =>
            _departmentRepository.Update(updatedDepartment.ToEntity());
        public bool RemoveDepartment(int id)
        {
            var dept = _departmentRepository.GetById(id);
            if (dept is null) return false;

            int delete = _departmentRepository.Remove(dept);
            return delete > 0 ? true : false;
        }
    }
}
