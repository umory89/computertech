using System;
using System.Collections.Generic;

namespace курсовая2511.TechAccounting.Application.Dtos.DTOs
{
    public class EquipmentSummaryDto
    {
        public int InStockCount { get; set; }
        public int IssuedCount { get; set; }
        public int UnderRepairCount { get; set; }
        public int WrittenOffCount { get; set; }
        public Dictionary<Guid, int> CountByType { get; set; } = new Dictionary<Guid, int>();
    }
}
