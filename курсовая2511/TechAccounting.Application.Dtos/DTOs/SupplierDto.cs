using System;

namespace курсовая2511.TechAccounting.Application.Dtos.DTOs
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string ContactPerson { get; set; } 
        public string Phone { get; set; } 
        public string Email { get; set; }
    }
}
