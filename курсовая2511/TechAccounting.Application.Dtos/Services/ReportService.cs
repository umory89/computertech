using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAccounting.Data;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;
using курсовая2511.TechAccounting.Application.Dtos.Interfaces;

namespace курсовая2511.TechAccounting.Application.Dtos.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context) => _context = context;

        public async Task<EquipmentSummaryDto> GetEquipmentSummaryAsync()
        {
            var allEquipment = await _context.Equipment.ToListAsync();

            return new EquipmentSummaryDto
            {
                InStockCount = allEquipment.Count(e => e.Status == "InStock"),
                IssuedCount = allEquipment.Count(e => e.Status == "Issued"),
                UnderRepairCount = allEquipment.Count(e => e.Status == "UnderRepair"),
                WrittenOffCount = allEquipment.Count(e => e.Status == "WrittenOff"),
                CountByType = allEquipment.GroupBy(e => e.EquipmentTypeId).ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}
