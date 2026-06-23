using System;

namespace курсовая2511.Models
{
    public class AssignmentHistory
    {
        public Guid Id { get; set; } 

        public string Actiontype { get; set; }
        public DateTime ActionDate { get; set; }
        public string PerformedBy { get; set; }
        public string Details { get; set; }
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        public Guid? EmployeeId { get; set; } 
        public Employee Employee { get; set; }
    }
}
