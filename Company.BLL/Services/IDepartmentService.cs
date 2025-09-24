using Company.BLL.DataTransferObjects;

namespace Company.BLL.Services
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