using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Domain.Models
{
    public class Media
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }

        public Media() { }
    }
}
