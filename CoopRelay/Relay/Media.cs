using CoopRelay.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;

namespace CoopRelay.Relay
{
    public static class Media
    {
        private static String GetKey(int id)
        {
            return string.Format("CoopRelay.Relay.Media.Get_{0}", id);
        }

        public static void ForceExpire(int id)
        {
            Cache.ForceExpire(GetKey(id));
        }

        public static CoopRelay.Domain.Models.Media Get(int id)
        {
            var key = GetKey(id);
            var m = Cache.Get<CoopRelay.Domain.Models.Media>(key);
            if(m==null)
            {
                var h = new Umbraco.Web.UmbracoHelper(Umbraco.Web.UmbracoContext.Current);
                m = Map(h.TypedMedia(id));
                Cache.Insert(key, m, AppSettings.CacheInterval);
            }
            return m;
        }

        public static List<CoopRelay.Domain.Models.Media> GetList(string csv)
        {
            var media = new List<CoopRelay.Domain.Models.Media>();
            var tmps = csv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int id = 0;
            foreach (var tmp in tmps)
            {
                if (int.TryParse(tmp, out id))
                {
                    var m = Get(id);
                    if (m != null)
                    {
                        media.Add(m);
                    }
                }
            }
            return media;
        }

        private static CoopRelay.Domain.Models.Media Map(Umbraco.Core.Models.IPublishedContent ipc)
        {
            return new Domain.Models.Media(){
                Id = ipc.Id,
                Name = ipc.Name,
                Url = ipc.Url
            };
        }
    }
}
