using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсовая2511.Models
{
    internal class EquipmentAssignment
    {
        public Guid Id { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        public string ConditionAtIssue { get; set; } = string.Empty;
        public string ConditionAtReturn { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public Guid EmployeeId { get; set; }
        public Guid EquipmentId { get; set; }

        public Employee Employee { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
        public void Close(DateTime returnDate, string condition)
        {
            ReturnDate = returnDate;
            ConditionAtReturn = condition;
        }
    }
}
