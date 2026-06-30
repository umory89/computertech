using System;

namespace курсовая2511.TechAccounting.Application.Dtos.DTOs
{
    public class EquipmentDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string InventoryNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string EquipmentTypeName { get; set; }
        public string SupplierName { get; set; }
    }

}
