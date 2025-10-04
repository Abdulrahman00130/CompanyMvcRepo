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
        private readonly Lazy<IDepartmentRepository> departmentRepository;
        private readonly Lazy<IEmployeeRepository> employeeRepository;
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.departmentRepository = new Lazy<IDepartmentRepository>(() => new DepartmentRepository(appDbContext));
            this.employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(appDbContext));
            this._appDbContext = appDbContext;
        }
        public IDepartmentRepository DepartmentRepository => departmentRepository.Value;

        public IEmployeeRepository EmployeeRepository => employeeRepository.Value;

        public int SaveChanges() => _appDbContext.SaveChanges();
    }
}
