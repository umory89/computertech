using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсовая2511.Models
{
    internal class Employee
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
    }
}
