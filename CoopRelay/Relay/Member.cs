using CoopRelay.Tools;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;

namespace CoopRelay.Relay
{
    public static class Member
    {
        public static CoopRelay.Domain.Models.Member Current
        {
            get 
            {
                return Get((int)System.Web.Security.Membership.GetUser().ProviderUserKey);
            }
        }

        private static String GetKey(int id)
        {
            return string.Format("CoopRelay.Relay.Member.Get_{0}", id);
        }

        public static void ForceExpireGet(int id)
        {
            Cache.ForceExpire(GetKey(id));
        }

        public static int Create(string username, string password, string email, string name, string memberTypeAlias, NameValueCollection properties)
        {
            var ms = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
            var m = ms.CreateMember(username,email,name,memberTypeAlias);
            foreach (string s in properties)
                foreach (string v in properties.GetValues(s))
                    m.SetValue(s,v);
            ms.Save(m);
            SetPassword(username, password);
            return m.Id;
        }

        public static bool IsExist(string username)
        {
            var ms = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
            return ms.Exists(username);
        }

        public static CoopRelay.Domain.Models.Member Get(int id)
        {
            if (id < 1) { return null; }

            var key = GetKey(id);
            var m = Cache.Get<CoopRelay.Domain.Models.Member>(key);
            if(m==null)
            {
                var ms = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
                m = Map(ms.GetById(id));
                Cache.Insert(key, m, AppSettings.CacheInterval);
            }
            return m;
        }

        public static int Update(string username, string name, string email, NameValueCollection nvc)
        {
            var ms = Umbraco.Core.ApplicationContext.Current.Services.MemberService;
            var m = ms.GetByUsername(username);
            m.Name = name;
            m.Email = email;
            foreach (string s in nvc)
                foreach (string v in nvc.GetValues(s))
                    m.SetValue(s, v);
            ms.Save(m);
            return m.Id;
        }

        public static void SetPassword(string username, string password)
        {
            var user = System.Web.Security.Membership.GetUser(username);
            user.ChangePassword(user.ResetPassword(), password);
        }

        public static List<CoopRelay.Domain.Models.Member> GetList(string csv)
        {
            var members = new List<CoopRelay.Domain.Models.Member>();

            if (String.IsNullOrEmpty(csv)) { return members; }

            var tmps = csv.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int id = 0;
            foreach (var tmp in tmps)
            {
                if(int.TryParse(tmp, out id))
                {
                    var m = Get(id);
                    if (m != null)
                    {
                        members.Add(m);
                    }
                }
            }
            return members;
        }

        private static CoopRelay.Domain.Models.Member Map(Umbraco.Core.Models.IMember m)
        {
            if (m == null) { return null; }

            var member = new CoopRelay.Domain.Models.Member()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Username = m.Username,
                Properties = Property.Map(m.Properties),
                Groups = null
            };
            return member;
        }
    }
}
