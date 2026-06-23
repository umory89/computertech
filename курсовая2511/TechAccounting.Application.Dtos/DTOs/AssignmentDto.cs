using System;

namespace курсовая2511.TechAccounting.Application.DTOs
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime? ReturnDate { get; set; }


        public string ConditionAtIssue { get; set; } = string.Empty;

        public string ConditionAtReturn { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

      
        public Guid EmployeeId { get; set; }


        public Guid EquipmentId { get; set; }
    }
}
