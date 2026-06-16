using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсовая2511.Models
{
    internal class User
    {
        public Guid id { get; set; }
        public string Username { get; set; }

        public string passwordHash { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
