using Company.DAL.Data.Contexts;
using Company.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(IDepartmentRepository departmentRepository, IEmployeeRepository employeeRepository, AppDbContext appDbContext)
        {
            this._departmentRepository = departmentRepository;
            this._employeeRepository = employeeRepository;
            this._appDbContext = appDbContext;
        }
        public IDepartmentRepository DepartmentRepository => _departmentRepository;

        public IEmployeeRepository EmployeeRepository => _employeeRepository;

        public int SaveChanges() => _appDbContext.SaveChanges();
    }
}
