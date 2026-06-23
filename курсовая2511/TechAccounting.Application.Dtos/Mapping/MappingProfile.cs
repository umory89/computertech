using AutoMapper;
using курсовая2511.Models; 
using курсовая2511.TechAccounting.Application.DTOs; 

namespace курсовая2511.TechAccounting.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
    
            CreateMap<Equipment, EquipmentDto>().ReverseMap();
            CreateMap<Equipment, EquipmentSummaryDto>().ReverseMap();
            CreateMap<Equipment, EquipmentDetailDto>().ReverseMap();

            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<EquipmentAssignment, AssignmentDto>().ReverseMap();
            CreateMap<AssignmentHistory, AssignmentHistoryDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
        }
    }
}
