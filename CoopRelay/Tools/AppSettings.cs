using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopRelay.Tools
{
    public static class AppSettings
    {
        public static int CacheInterval
        {
            get { return Get<int>("CoopRelay._CacheInterval"); }
        }

        public static T Get<T>(string key)
        {
            var app = new System.Configuration.AppSettingsReader();
            try
            {
                return (T)app.GetValue(key, typeof(T));
            }
            catch { return default(T); }
        }
    }
}
