using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.DataTransferObjects
{
    public class DepartmentByIdDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateOnly? CreateDate { get; set; }
        public DateOnly? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
