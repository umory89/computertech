using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;

namespace курсовая2511.TechAccounting.Application.Dtos.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(Guid id);
        Task CreateAsync(EmployeeDto dto);
        Task UpdateAsync(EmployeeDto dto);
        Task DeleteAsync(Guid id);
    }
}
