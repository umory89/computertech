using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;
using курсовая2511.TechAccounting.Application.DTOs;

namespace курсовая2511.TechAccounting.Application.Dtos.Interfaces
{
    public interface IAssignmentService
    {
        Task<IEnumerable<AssignmentDto>> GetAllAsync();
        Task CreateAsync(AssignmentDto dto);
        Task ReturnEquipmentAsync(Guid id, string condition);
    }
}

