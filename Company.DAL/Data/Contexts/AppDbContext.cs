
using Company.DAL.Models.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Company.DAL.Data.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
