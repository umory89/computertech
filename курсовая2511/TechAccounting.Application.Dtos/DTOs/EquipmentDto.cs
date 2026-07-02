using System;

namespace курсовая2511.TechAccounting.Application.Dtos.DTOs
{
    public class EquipmentDto
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
        public Guid EquipmentTypeId { get; set; }
        public Guid SupplierId { get; set; }
    }
}