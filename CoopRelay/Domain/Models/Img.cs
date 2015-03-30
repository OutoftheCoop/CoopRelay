using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Domain.Models
{
    public class Img
    {
        public String Src { get; set; }
        public String Alt { get; set; }

        public Img(String src, String alt)
        {
            Src = src;
            Alt = alt;
        }

        public override String ToString()
        {
            return ToString(new Dictionary<string, string>());
        }

        public String ToString(bool selflink)
        {
            if (string.IsNullOrEmpty(Src)) { return string.Empty; }

            return new Anchor(Src, Alt).ToString(ToString(), false, null);
        }

        public String ToString(Dictionary<String, String> attributes)
        {
            if (string.IsNullOrEmpty(Src)) { return string.Empty; }

            var sb = new StringBuilder("<img");
            if (attributes == null)
            {
                attributes = new Dictionary<string, string>();
            }
            if (!attributes.ContainsKey("src"))
                attributes.Add("src", Src);
            if (!attributes.ContainsKey("alt"))
                attributes.Add("alt", Alt);
            foreach (var att in attributes)
            {
                sb.AppendFormat(" {0}=\"{1}\"",
                    System.Web.HttpUtility.HtmlEncode(att.Key),
                    System.Web.HttpUtility.HtmlEncode(att.Value)
                    );
            }
            sb.Append("/>");
            return sb.ToString();
        }

        public String ToString(int size)
        {
            return ToString(new Dictionary<string, string>(), size);
        }

        public String ToString(bool selflink, int size)
        {
            if (string.IsNullOrEmpty(Src)) { return string.Empty; }

            return new Anchor(Src, Alt).ToString(ToString(size), false, null);
        }

        public String ToString(Dictionary<String, String> attributes, int size)
        {
            var tmp = Src;
            Src = MakeThumbSrc(size);
            var result = ToString(attributes);
            Src = tmp;
            return result;
        }

        private String MakeThumbSrc(int size)
        {
            if (String.IsNullOrEmpty(Src) || Src.LastIndexOf('.') < 0) { return Src; }

            return Src.Substring(0, Src.LastIndexOf('.')) + string.Format("_thumb_{0}.jpg", size);
        }
    }
}
