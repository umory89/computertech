using System;

namespace курсовая2511.TechAccounting.Application.Dtos.DTOs
{
    public class AssignmentHistoryDto
    {
        public Guid Id { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string PerformedBy { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public Guid EquipmentId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
