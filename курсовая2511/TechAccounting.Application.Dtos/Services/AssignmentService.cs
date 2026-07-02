using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechAccounting.Data;
using курсовая2511.Models;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;
using курсовая2511.TechAccounting.Application.Dtos.Interfaces;
using курсовая2511.TechAccounting.Application.DTOs;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _context;
        public AssignmentService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<AssignmentDto>> GetAllAsync()
        {
            var items = await _context.EquipmentAssignments.ToListAsync();
            return items.Select(a => new AssignmentDto
            {
                Id = a.Id,
                AssignedDate = a.AssignedDate,
                ReturnDate = a.ReturnDate,
                ConditionAtIssue = a.ConditionAtIssue,
                ConditionAtReturn = a.ConditionAtReturn,
                Notes = a.Notes,
                EmployeeId = a.EmployeeId,
                EquipmentId = a.EquipmentId
            });
        }

        public async Task CreateAsync(AssignmentDto dto)
        {
            var equipment = await _context.Equipment.FindAsync(dto.EquipmentId);
            if (equipment == null || equipment.Status != "InStock")
                throw new InvalidOperationException("Техника недоступна для выдачи.");

            var assignment = new EquipmentAssignment
            {
                Id = Guid.NewGuid(),
                AssignedDate = DateTime.UtcNow,
          
                ConditionAtIssue = dto.ConditionAtIssue ?? string.Empty,
                ConditionAtReturn = string.Empty,
                Notes = dto.Notes ?? string.Empty,
                EmployeeId = dto.EmployeeId,
                EquipmentId = dto.EquipmentId
            };

            equipment.Status = "Issued";
            _context.EquipmentAssignments.Add(assignment);
            _context.Equipment.Update(equipment);

            _context.AssignmentHistories.Add(new AssignmentHistory
            {
                Id = Guid.NewGuid(),
                EquipmentId = dto.EquipmentId,
                EmployeeId = dto.EmployeeId,
                ActionType = "Выдача",
                ActionDate = DateTime.UtcNow,
                PerformedBy = "Admin",
                Details = $"Выдано сотруднику. Состояние: {assignment.ConditionAtIssue}"
            });

            await _context.SaveChangesAsync();
        }

        public async Task ReturnEquipmentAsync(Guid id, string condition)
        {
            var assignment = await _context.EquipmentAssignments.FindAsync(id);
            if (assignment == null || assignment.ReturnDate != null)
                throw new InvalidOperationException("Операция невозможна.");

            assignment.ReturnDate = DateTime.UtcNow;
            assignment.ConditionAtReturn = condition ?? string.Empty;

            var equipment = await _context.Equipment.FindAsync(assignment.EquipmentId);
            if (equipment != null) equipment.Status = "InStock";

            _context.EquipmentAssignments.Update(assignment);
            _context.AssignmentHistories.Add(new AssignmentHistory
            {
                Id = Guid.NewGuid(),
                EquipmentId = assignment.EquipmentId,
                EmployeeId = assignment.EmployeeId,
                ActionType = "Возврат",
                ActionDate = DateTime.UtcNow,
                PerformedBy = "Admin",
                Details = $"Возвращено на склад. Состояние: {assignment.ConditionAtReturn}"
            });

            await _context.SaveChangesAsync();
        }
    }
}
