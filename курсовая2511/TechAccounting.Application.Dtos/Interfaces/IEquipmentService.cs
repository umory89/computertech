using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;

namespace курсовая2511.TechAccounting.Application.Dtos.Interfaces
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentDto>> GetAllAsync();
        Task<IEnumerable<EquipmentDetailDto>> GetAllDetailsAsync();
        Task<EquipmentDto?> GetByIdAsync(Guid id);
        Task CreateAsync(EquipmentDto dto);
        Task UpdateAsync(EquipmentDto dto);
        Task DeleteAsync(Guid id);
    }
}
