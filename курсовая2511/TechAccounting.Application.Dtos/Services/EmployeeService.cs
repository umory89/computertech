using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;
using курсовая2511.Models;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;
using курсовая2511.TechAccounting.Application.Dtos.Interfaces;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        public EmployeeService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var items = await _context.Employees.ToListAsync();
            return items.Select(ToDto);
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var e = await _context.Employees.FindAsync(id);
            return e == null ? null : ToDto(e);
        }

        public async Task CreateAsync(EmployeeDto dto)
        {
            _context.Employees.Add(new Employee
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                FirstName = dto.FirstName ?? string.Empty,
                LastName = dto.LastName ?? string.Empty,
                Department = dto.Department ?? string.Empty,
                Position = dto.Position ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                HireDate = dto.HireDate == default ? DateTime.Now : dto.HireDate,
                IsActive = true,
      
                UserId = (dto.UserId == Guid.Empty) ? (Guid?)null : dto.UserId
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeDto dto)
        {
            var entity = await _context.Employees.FindAsync(dto.Id);
            if (entity != null)
            {
                entity.FirstName = dto.FirstName; entity.LastName = dto.LastName;
                entity.Department = dto.Department; entity.Position = dto.Position;
                entity.Email = dto.Email; entity.Phone = dto.Phone; entity.IsActive = dto.IsActive;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity != null) { _context.Employees.Remove(entity); await _context.SaveChangesAsync(); }
        }

        private static EmployeeDto ToDto(Employee e) => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Department = e.Department,
            Position = e.Position,
            Email = e.Email,
            Phone = e.Phone,
            HireDate = e.HireDate,
            IsActive = e.IsActive,
            UserId = e.UserId ?? Guid.Empty
        };
    }
}
