using CoopRelay.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Umbraco.Core.Services;

namespace CoopRelay.Relay
{
    public static class MemberGroup
    {
        public static String GetListKey(string username)
        {
            return string.Format("CoopRelay.Relay.MemberGroup.GetList_{0}", username);
        }

        public static String[] GetList(string username)
        {
            var key = GetListKey(username);
            var mgs = Cache.Get<String[]>(key);
            if(mgs==null)
            {
                mgs = Roles.GetRolesForUser(username);
                Cache.Insert(key, mgs, AppSettings.CacheInterval);
            }
            return mgs;
        }

        public static String[] GetList()
        {
            return Roles.GetAllRoles();
        }

        public static bool IsMemberInGroup(CoopRelay.Domain.Models.Member m, string group)
        {
            return Roles.IsUserInRole(m.Username, group);
        }

        public static bool IsMemberInAnyGroup(CoopRelay.Domain.Models.Member m, string[] groups)
        {
            foreach (var g in groups)
            {
                if (IsMemberInGroup(m, g))
                    return true;
            }
            return false;
        }

        public static bool GroupExists(string group)
        {
            return GetList().Contains(group);
        }
    }
}
