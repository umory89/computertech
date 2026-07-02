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
            return items.Select(e => new EmployeeDto
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
                UserId = e.UserId
            });
        }

        public async Task<EmployeeDto?> GetByIdAsync(Guid id)
        {
            var e = await _context.Employees.FindAsync(id);
            if (e == null) return null;
            return new EmployeeDto
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
                UserId = e.UserId
            };
        }

        public async Task CreateAsync(EmployeeDto dto)
        {
            _context.Employees.Add(new Employee
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Department = dto.Department,
                Position = dto.Position,
                Email = dto.Email,
                Phone = dto.Phone,
                HireDate = dto.HireDate,
                IsActive = dto.IsActive,
                UserId = dto.UserId
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
    }
}