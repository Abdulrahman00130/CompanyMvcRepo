using Company.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repositories
{
    public class DepartmentRepository(AppDbContext _context) : IDepartmentRepository
    {
        public IEnumerable<Department> GetAll(bool isTracked = false)
        {
            if (isTracked)
                return _context.Departments.ToList();
            else
                return _context.Departments.AsNoTracking().ToList();
        }

        public Department? GetById(int id)
        {
            return _context.Departments.Find(id);
        }

        public int Add(Department dept)
        {
            if (dept is null) return 0;
            _context.Add(dept);
            return _context.SaveChanges();
        }
        public int Update(Department dept)
        {
            if (dept is null) return 0;
            _context.Update(dept);
            return _context.SaveChanges();
        }
        public int Remove(Department dept)
        {
            if (dept is null) return 0;
            _context.Remove(dept);
            return _context.SaveChanges();
        }
    }
}
