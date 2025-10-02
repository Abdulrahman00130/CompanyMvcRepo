using Company.DAL.Models.Shared;
using Company.DAL.Models.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models.EmployeeModel
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; }
        public Gender Gender { get; set; }
        public EmployeeType EmployeeType { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        
    }
}
