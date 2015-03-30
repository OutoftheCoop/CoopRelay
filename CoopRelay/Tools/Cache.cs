using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Tools
{
    public static class Cache
    {
        public static void ClearCache(string contains)
        {
            var c = System.Web.HttpContext.Current.Cache;

            foreach (DictionaryEntry item in c)
            {
                var key = item.Key.ToString();
                if (key.Contains(contains))
                    ForceExpire(key);
            }
        }

        public static void ForceExpire(string key)
        {
            var c = System.Web.HttpContext.Current.Cache;
            c.Remove(key);
        }

        public static void Insert(string key, object obj, int minutes)
        {
            var c = System.Web.HttpContext.Current.Cache;
            c.Insert(key,obj,null,DateTime.Now.AddMinutes(minutes),System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static T Get<T>(string key)
        {
            try
            {
                var c = System.Web.HttpContext.Current.Cache;
                return (T)c.Get(key);
            }
            catch { return default(T); }
        }
    }
}
