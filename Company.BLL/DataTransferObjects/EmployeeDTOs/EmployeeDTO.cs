using Company.DAL.Models.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.DataTransferObjects.EmployeeDTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [DataType(DataType.Currency)]
        public double Salary { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public string EmpGender { get; set; }

        [Display(Name = "Employee Type")]
        public string EmpType { get; set; }
        public string? Department { get; set; }

    }
}
