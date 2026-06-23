using System;
using System.Collections.Generic;

namespace курсовая2511.Models
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Equipment> Equipment { get; set; }
    }
}
