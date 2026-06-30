using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;
using курсовая2511.Models;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{

    public class SupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var items = await _context.Suppliers.ToListAsync();
            return items.Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                ContactPerson = s.ContactPerson,
                Phone = s.Phone,
                Email = s.Email
            });
        }

        public async Task CreateAsync(SupplierDto dto)
        {
            _context.Suppliers.Add(new Supplier
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Name = dto.Name,
                ContactPerson = dto.ContactPerson,
                Phone = dto.Phone,
                Email = dto.Email
            });
            await _context.SaveChangesAsync();
        }
    }
}
