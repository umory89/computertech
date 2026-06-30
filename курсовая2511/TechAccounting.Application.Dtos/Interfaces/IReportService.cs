using System.Threading.Tasks;
using курсовая2511.TechAccounting.Application.Dtos.DTOs;

namespace курсовая2511.TechAccounting.Application.Dtos.Interfaces
{
    public interface IReportService
    {
        Task<EquipmentSummaryDto> GetEquipmentSummaryAsync();
    }
}
