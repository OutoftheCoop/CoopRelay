using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Domain.Models
{
    public class PropertyCollection : List<Property>
    {
        public T GetAlias<T>(string alias)
        {
            try
            {
                return (T)this.SingleOrDefault(p => p.Alias.Equals(alias)).Value;
            }
            catch { return default(T); }
        }

        public Property GetProperty(string alias)
        {
            return this.SingleOrDefault(p => p.Alias.Equals(alias));
        }

        public string GetString(string alias)
        {
            try
            {
                return this.SingleOrDefault(p => p.Alias.Equals(alias)).Value.ToString();
            }
            catch { return string.Empty; }
        }

        public string[] GetStringArray(string name)
        {
            return GetString(name).Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public List<int> GetIntList(string name)
        {
            return GetStringArray(name).Select(n => int.Parse(n)).ToList();
        }

        public Media GetFirstMedia(string name)
        {
            var ids = GetIntList(name);

            if (ids == null || ids.Count() == 0) { return null; }

            return CoopRelay.Relay.Media.Get(ids[0]);
        }

        public List<Media> GetMedia(string name)
        {
            var media = new List<Media>();
            var ids = GetIntList(name);
            foreach (var id in ids)
            {
                var m = CoopRelay.Relay.Media.Get(id);
                if (m != null)
                {
                    media.Add(m);
                }
            }
            return media;
        }

        public Content GetFirstContent(string name)
        {
            var ids = GetIntList(name);

            if (ids == null || ids.Count() == 0) { return null; }

            return CoopRelay.Relay.Content.Get(ids[0]);
        }

        public List<Content> GetContent(string name)
        {
            var content = new List<Content>();
            var ids = GetIntList(name);
            foreach (var id in ids)
            {
                var c = CoopRelay.Relay.Content.Get(id);
                if (c != null)
                {
                    content.Add(c);
                }
            }
            return content;
        }

        public DateTime GetDateTime(string name)
        {
            var s = GetString(name);
            DateTime d;
            DateTime.TryParse(s, out d);
            return d;
        }

        public Boolean GetBoolean(string name)
        {
            var s = GetString(name);
            Boolean b;
            Boolean.TryParse(s, out b);
            return b;
        }

        public Int32 GetInt(string name)
        {
            var s = GetString(name);
            Int32 i;
            Int32.TryParse(s, out i);
            return i;
        }

        public Img GetFirstImg(string name)
        {
            var m = GetFirstMedia(name);

            if (m == null) { return new Img("", ""); }

            return new Img(m.Url, m.Name);
        }

        public List<Img> GetImgList(string name)
        {
            var images = new List<Img>();

            var media = GetMedia(name);

            if (media == null || media.Count == 0) { return images; }

            foreach (var m in media)
            {
                images.Add(new Img(m.Url, m.Name));
            }

            return images;
        }

        public List<Anchor> GetMenu(string name)
        {
            var menu = new List<Anchor>();

            var content = GetContent(name);
            foreach (var c in content)
            {
                menu.Add(c.Quicklink);
            }

            return menu;
        }
    }
}
