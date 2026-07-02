using System;
using System.Collections.Generic;

namespace курсовая2511.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<EquipmentAssignment> Assignments { get; set; }
        public ICollection<AssignmentHistory> History { get; set; }
    }
}