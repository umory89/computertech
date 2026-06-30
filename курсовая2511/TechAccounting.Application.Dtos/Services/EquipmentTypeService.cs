using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;
using курсовая2511.Models;
using курсовая2511.TechAccounting.Application.Dtos.Interfaces;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly AppDbContext _context;

        public EquipmentTypeService(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<EquipmentType>> GetAllAsync()
        {
            return await _context.EquipmentTypes.ToListAsync();
        }

       
        public async Task CreateAsync(EquipmentType entity)
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            _context.EquipmentTypes.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
