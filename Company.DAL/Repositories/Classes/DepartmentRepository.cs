using Company.DAL.Data.Contexts;
using Company.DAL.Repositories.Interfaces;

namespace Company.DAL.Repositories.Classes
{
    public class DepartmentRepository(AppDbContext _context) : GenericRepository<Department>(_context), IDepartmentRepository
    {
        
    }
}
