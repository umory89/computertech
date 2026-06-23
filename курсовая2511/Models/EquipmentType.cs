using System;
using System.Collections.Generic;

namespace курсовая2511.Models
{
    public class EquipmentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public ICollection<Equipment> Equipment { get; set; }
    }
}
