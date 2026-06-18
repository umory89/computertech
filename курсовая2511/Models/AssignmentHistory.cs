using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсовая2511.Models
{
    internal class AssignmentHistory
    {
        public Guid id { get; set; }

        public string Actiontype { get; set; }

        public DateTime ActionDate { get; set; }

        public string PerformedBy { get; set; }

        public string Details { get; set; }



    }
}
