using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Equipment Status { get; set; } 
        public string Notes { get; set; } 
        public int WarrantyMonths { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public DateTime CreatedAt { get; set; } 

    }
}
