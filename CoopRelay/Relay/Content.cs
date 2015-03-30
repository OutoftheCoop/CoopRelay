using CoopRelay.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;

namespace CoopRelay.Relay
{
    public static class Content
    {
        private static String GetKey(int id)
        {
            return string.Format("CoopRelay.Relay.Content.Get_{0}", id);
        }

        private static String GetChildrenKey(int id)
        {
            return string.Format("CoopRelay.Relay.Content.GetChildren_{0}", id);
        }

        private static String GetAllByTagKey(string tag)
        {
            return string.Format("CoopRelay.Relay.Content.GetAllByTag_{0}", tag);
        }

        public static void ForceExpireGet(int id)
        {
            Cache.ForceExpire(GetKey(id));
        }

        public static void ForceExpireGetChildren(int id)
        {
            Cache.ForceExpire(GetChildrenKey(id));
        }

        public static void ForceExpireGetAllByTag(string tag)
        {
            Cache.ForceExpire(GetAllByTagKey(tag));
        }

        public static void Create(string name, int parentid, string doctypealias, CoopRelay.Domain.Models.PropertyCollection pc)
        {
            var cs = Umbraco.Core.ApplicationContext.Current.Services.ContentService;
            var c = cs.CreateContent(name, parentid, doctypealias);
            foreach(var p in pc)
            {
                c.SetValue(p.Alias, p.Value);
            }
            cs.SaveAndPublishWithStatus(c);
        }

        public static CoopRelay.Domain.Models.Content Get(int id)
        {
            var key = GetKey(id);
            var c = Cache.Get<CoopRelay.Domain.Models.Content>(key);
            if(c==null)
            {
                var h = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
                c = Map(h.TypedContent(id));
                if(c!=null)
                {
                    Cache.Insert(key, c, AppSettings.CacheInterval);
                }
            }
            return c;
        }

        public static List<CoopRelay.Domain.Models.Content> GetAll(string content)
        {
            var ids = content.Split(',').Select(Int32.Parse).ToArray();

            var cs = new List<CoopRelay.Domain.Models.Content>();
            foreach(var id in ids)
            {
                cs.Add(Get(id));
            }
            return cs;
        }

        public static List<CoopRelay.Domain.Models.Content> GetChildren(int id, string documenttype)
        {
            var key = GetChildrenKey(id);
            var c = Cache.Get<List<CoopRelay.Domain.Models.Content>>(key);
            if (c == null)
            {
                var h = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
                c = Map(h.TypedContent(id).Children.Where(g => g.DocumentTypeAlias.Equals(documenttype)));
                Cache.Insert(key, c, AppSettings.CacheInterval);
            }
            return c;
        }

        public static List<CoopRelay.Domain.Models.Content> GetAllByTag(string tag)
        {
            var key = GetAllByTagKey(tag);
            var c = Cache.Get<List<CoopRelay.Domain.Models.Content>>(key);
            if (c == null)
            {
                c = new List<CoopRelay.Domain.Models.Content>();
                var ts = Umbraco.Core.ApplicationContext.Current.Services.TagService.GetTaggedContentByTag(tag, "menu");
                foreach (var t in ts)
                {
                    var tmp = Get(t.EntityId);
                    if(tmp!=null)
                        c.Add(tmp);
                }
                Cache.Insert(key, c, AppSettings.CacheInterval);
            }
            return c;
        }

        private static CoopRelay.Domain.Models.Content Map(Umbraco.Core.Models.IPublishedContent ic)
        {
            if (ic == null) { return null; }

            return new Domain.Models.Content(){
                Id = ic.Id,
                Name = ic.Name,
                ParentId = ic.Path.Split(new char[] { ',' }).Select(s => int.Parse(s)).Reverse().Skip(1).Take(1).SingleOrDefault(),
                Url = ic.Url,
                Properties = CoopRelay.Relay.Property.Map(ic.Properties)
            };
        }

        private static List<CoopRelay.Domain.Models.Content> Map(IEnumerable<Umbraco.Core.Models.IPublishedContent> ics)
        {
            var content = new List<CoopRelay.Domain.Models.Content>();
            foreach (var ic in ics)
            {
                var c = Map(ic);
                if (c != null)
                {
                    content.Add(c);
                }
            }
            return content;
        }
    }
}
