using Company.BLL.DataTransferObjects.EmployeeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Services.Interfaces
{
    public interface IEmployeeService
    {
        public IEnumerable<EmployeeDTO> GetAllEmployees(bool isTracked = false);
        public EmployeeDetailsDTO GetById(int id);
        public int CreateEmployee(CreatedEmployeeDTO employeeDTO);
        public int UpdateEmployee(UpdatedEmployeeDTO employeeDTO);
        public bool DeleteEmployee(int id);
    }
}
