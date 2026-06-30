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
    public class EquipmentService : IEquipmentService
    {
        private readonly AppDbContext _context;

        public EquipmentService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<EquipmentDto>> GetAllAsync()
        {
            var items = await _context.Equipment.ToListAsync();
            return items.Select(e => new EquipmentDto
            {
                Id = e.Id,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                InventoryNumber = e.InventoryNumber,
                Manufacturer = e.Manufacturer,
                Model = e.Model,
                PurchaseDate = e.PurchaseDate,
                Price = e.Price,
                Location = e.Location,
                Status = e.Status,
                Notes = e.Notes,
                WarrantyMonths = e.WarrantyMonths,
                WarrantyEndDate = e.WarrantyEndDate,
                EquipmentTypeId = e.EquipmentTypeId,
                SupplierId = e.SupplierId
            });
        }

        public async Task<IEnumerable<EquipmentDetailDto>> GetAllDetailsAsync()
        {
 
            var items = await _context.Equipment
                .Include(e => e.EquipmentType)
                .Include(e => e.Supplier)
                .ToListAsync();

            return items.Select(e => new EquipmentDetailDto
            {
                Id = e.Id,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                InventoryNumber = e.InventoryNumber,
                Manufacturer = e.Manufacturer,
                Model = e.Model,
                Status = e.Status,
                Location = e.Location,
                EquipmentTypeName = e.EquipmentType?.Name ?? "Не указан",
                SupplierName = e.Supplier?.Name ?? "Не указан"
            });
        }

        public async Task<EquipmentDto?> GetByIdAsync(Guid id)
        {
            var e = await _context.Equipment.FindAsync(id);
            if (e == null) return null;
            return new EquipmentDto { Id = e.Id, Name = e.Name, SerialNumber = e.SerialNumber, InventoryNumber = e.InventoryNumber, Status = e.Status, EquipmentTypeId = e.EquipmentTypeId, SupplierId = e.SupplierId };
        }

        public async Task CreateAsync(EquipmentDto dto)
        {
            _context.Equipment.Add(new Equipment
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Name = dto.Name,
                SerialNumber = dto.SerialNumber,
                InventoryNumber = dto.InventoryNumber,
                Manufacturer = dto.Manufacturer,
                Model = dto.Model,
                PurchaseDate = dto.PurchaseDate,
                Price = dto.Price,
                Location = dto.Location,
                Status = "InStock",
                Notes = dto.Notes,
                WarrantyMonths = dto.WarrantyMonths,
                WarrantyEndDate = dto.WarrantyEndDate,
                EquipmentTypeId = dto.EquipmentTypeId,
                SupplierId = dto.SupplierId
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EquipmentDto dto)
        {
            var entity = await _context.Equipment.FindAsync(dto.Id);
            if (entity != null)
            {
                entity.Name = dto.Name; entity.SerialNumber = dto.SerialNumber; entity.InventoryNumber = dto.InventoryNumber;
                entity.Status = dto.Status; entity.Location = dto.Location;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Equipment.FindAsync(id);
            if (entity != null) { _context.Equipment.Remove(entity); await _context.SaveChangesAsync(); }
        }
    }
}
