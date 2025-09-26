using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.DataTransferObjects
{
    public class CreatedDepartmentDTO
    {
        [Required(ErrorMessage = "Name is required!!!")]
        public string Name { get; set; }
        [Required]
        [Range(100, int.MaxValue)]
        public string Code { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateOnly? CreateDate { get; set; }

    }
}
