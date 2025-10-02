using Company.DAL.Models.EmployeeModel;
using Company.DAL.Models.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.DataTransferObjects.EmployeeDTOs
{
    public class UpdatedEmployeeDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max length should be 50 character")]
        [MinLength(5, ErrorMessage = "Min length should be 5 characters")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int? Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{4,10}-[a-zA-Z]{4,10}-[a-zA-Z]{4,10}$",
           ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string? Address { get; set; }

        [DataType(DataType.Currency)]
        public double Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateOnly HiringDate { get; set; }
        public Gender Gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
    }
}
