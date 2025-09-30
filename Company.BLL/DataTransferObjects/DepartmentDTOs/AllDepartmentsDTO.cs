using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.DataTransferObjects.DepartmentDTOs
{
    public class AllDepartmentsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateOnly? CreateDate { get; set; }
    }
}
