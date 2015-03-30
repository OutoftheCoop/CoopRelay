using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Domain.Models
{
    public class Content
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Int32 ParentId { get; set; }
        public String Url { get; set; }
        public PropertyCollection Properties { get; set; }

        public Content() { }

        public Anchor Quicklink
        {
            get
            {
                return new Anchor(Url, Properties.GetString("title"));
            }
        }
    }
}
