using Company.BLL.DataTransferObjects;
using Company.DAL.Models.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Factories
{
    public static class DepartmentFactory
    {
        public static AllDepartmentsDTO? ToAllDepartmentsDTO(this Department d)
        {
            if (d is null) return null;
            return new AllDepartmentsDTO()
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Description = d.Description ?? string.Empty,
                CreateDate = DateOnly.FromDateTime(d.CreatedOn ?? new DateTime())
            };
        }
        public static DepartmentByIdDTO? ToDepartmentByIdDTO(this Department d)
        {
            if (d is null) return null;
            return new DepartmentByIdDTO()
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Description = d.Description ?? string.Empty,
                CreatedBy = d.CreatedBy,
                LastModifiedBy = d.LastModifiedBy,
                CreateDate = DateOnly.FromDateTime(d.CreatedOn ?? new DateTime()),
                LastModifiedDate = DateOnly.FromDateTime(d.LastModifiedOn ?? new DateTime()),
                IsDeleted = d.IsDeleted
            };
        }

        public static Department ToEntity(this CreatedDepartmentDTO d) =>
            new Department()
            {
                Name = d.Name,
                Code = d.Code,
                Description = d.Description ?? string.Empty,
                CreatedOn = new DateTime(d.CreateDate ?? DateOnly.FromDateTime(DateTime.Now), new TimeOnly())
            };

        public static Department ToEntity(this UpdatedDepartmentDTO d) =>
            new Department()
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                Description = d.Description ?? string.Empty,
                CreatedOn = new DateTime(d.CreateDate ?? new DateOnly(), new TimeOnly())
            };
    }
}
