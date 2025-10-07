using Company.BLL.DataTransferObjects.DepartmentDTOs;

namespace Company.BLL.Services.Interfaces
{
    public interface IDepartmentService
    {
        int AddDepartment(CreatedDepartmentDTO createdDepartment);
        IEnumerable<AllDepartmentsDTO> GetAllDepartments();
        DepartmentByIdDTO? GetDepartmentById(int id);
        bool RemoveDepartment(int id);
        int UpdateDepartment(UpdatedDepartmentDTO updatedDepartment);
    }
}