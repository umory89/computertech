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
        public DateTime AssignedDate { get; set; } 
        public DateTime? ReturnDate { get; set; }
        public string ConditionAtIssue { get; set; } 
        public string ConditionAtReturn { get; set; } 
        public string Notes { get; set; } 
  

    
     
    }
}


