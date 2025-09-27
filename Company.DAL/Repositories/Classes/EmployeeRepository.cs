using Company.DAL.Data.Contexts;
using Company.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repositories.Classes
{
    public class EmployeeRepository(AppDbContext _context) : GenericRepository<Employee>(_context), IEmployeeRepository
    {
    }
}
