using System;
using System.Collections.Generic;

namespace курсовая2511.Models
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string InventoryNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } 
        public string Notes { get; set; }
        public int WarrantyMonths { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public ICollection<EquipmentAssignment> Assignments { get; set; }
        public ICollection<AssignmentHistory> History { get; set; }
    }
}
