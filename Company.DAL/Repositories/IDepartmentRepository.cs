
namespace Company.DAL.Repositories
{
    public interface IDepartmentRepository
    {
        int Add(Department dept);
        IEnumerable<Department> GetAll(bool isTracked = false);
        Department? GetById(int id);
        int Remove(Department dept);
        int Update(Department dept);
    }
}