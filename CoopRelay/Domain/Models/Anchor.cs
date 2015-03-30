using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Domain.Models
{
    public class Anchor
    {
        public String Href { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }

        public Anchor(String href, String text_title)
        {
            Href = href;
            Title = Text = text_title;
        }

        public override String ToString()
        {
            return ToString(Text, true, null);
        }

        public String ToString(Dictionary<String, String> attributes)
        {
            return ToString(Text, true, attributes);
        }

        public String ToString(String text, Boolean encode)
        {
            return ToString(text, encode, null);
        }

        public String ToString(String text, Boolean encode, Dictionary<String, String> attributes)
        {
            var sb = new StringBuilder("<a");
            if (attributes == null)
            {
                attributes = new Dictionary<string, string>();
            }
            if (!attributes.ContainsKey("href"))
                attributes.Add("href", Href);
            if (!attributes.ContainsKey("title"))
                attributes.Add("title", Title);
            foreach (var att in attributes)
            {
                sb.AppendFormat(" {0}=\"{1}\"",
                    System.Web.HttpUtility.HtmlEncode(att.Key),
                    System.Web.HttpUtility.HtmlEncode(att.Value)
                    );
            }
            sb.AppendFormat(">{0}</a>",
                encode ? System.Web.HttpUtility.HtmlEncode(text) : text
                );
            return sb.ToString();
        }
    }
}
