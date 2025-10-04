using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Company.BLL.DataTransferObjects.EmployeeDTOs;
using Company.DAL.Models.EmployeeModel;

namespace Company.BLL.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.EmpType, options => options.MapFrom(src => src.EmployeeType))
                .ForMember(dest => dest.EmpGender, options => options.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Department, options => options.MapFrom(src => src.Department == null ? null : src.Department.Name));

            CreateMap<Employee, EmployeeDetailsDTO>()
                .ForMember(dest => dest.EmployeeType, options => options.MapFrom(src => src.EmployeeType))
                .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender))
                .ForMember(dest => dest.HiringDate, options => options.MapFrom(src => DateOnly.FromDateTime(src.HiringDate)))
                .ForMember(dest => dest.Department, options => options.MapFrom(src => src.Department == null ? null : src.Department.Name));

            CreateMap<CreatedEmployeeDTO, Employee>()
                .ForMember(dest => dest.HiringDate, options => options.MapFrom(src => src.HiringDate.ToDateTime(new TimeOnly())));

            CreateMap<UpdatedEmployeeDTO, Employee>()
                .ForMember(dest => dest.HiringDate, options => options.MapFrom(src => src.HiringDate.ToDateTime(new TimeOnly())));

        }
    }
}
