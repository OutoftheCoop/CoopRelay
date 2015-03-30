using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Domain.Models
{
    public class Member
    {
        public Member() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public PropertyCollection Properties { get; set; }
        public String[] Groups { get; set; }
    }
}
