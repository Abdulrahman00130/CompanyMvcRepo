using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.DataTransferObjects
{
    public class CreatedDepartmentDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateOnly? CreateDate { get; set; }

    }
}
