using CoopRelay.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examine;
using Examine.SearchCriteria;

namespace CoopRelay.Relay
{
    public static class Search
    {
        private static String GetKey(string[] terms, string[] fields, int excludeid, int parentid, int limit, bool shuffle)
        {
            return string.Format("CoopRelay.Relay.Search.Get_{0}_{1}_{2}_{3}_{4}_{5}", terms.Join(), fields.Join(), excludeid, parentid, limit, shuffle);
        }

        public static List<CoopRelay.Domain.Models.Content> Get(string[] terms, string[] fields, int excludeid, int parentid, int limit, bool shuffle)
        {
            var key = GetKey(terms, fields, excludeid, parentid, limit, shuffle);
            var s = Cache.Get<ISearchResults>(key);
            if (s == null)
            {
                var searcher = ExamineManager.Instance;
                var searchCriteria = searcher.CreateSearchCriteria();
                var query = searchCriteria.GroupedOr(fields, terms);
                if(excludeid>0)
                    query = query.Not().Id(excludeid);
                if(parentid>0)
                    query = query.And().ParentId(parentid);
                var fullquery = query.Compile();
                s = searcher.Search(fullquery);
                if (s != null)
                {
                    Cache.Insert(key, s, AppSettings.CacheInterval);
                }
            }

            IEnumerable<SearchResult> finalform = s;

            if(shuffle)
            {
                var random = new Random();
                var randomNumbers = s.Select(r => random.Next()).ToArray();
                finalform = s.Zip(randomNumbers, (r, o) => new { Result = r, Order = o })
                    .OrderBy(o => o.Order)
                    .Select(o => o.Result);
            }

            if(limit>0)
            {
                finalform = finalform.Take(limit);
            }

            return Map(finalform);
        }

        private static List<Domain.Models.Content> Map(IEnumerable<SearchResult> s)
        {
            var c = new List<Domain.Models.Content>();
            foreach(var r in s)
            {
                c.Add(Map(r));
            }
            return c;
        }

        private static Domain.Models.Content Map(SearchResult s)
        {
            return Content.Get(s.Id);
        }

        private static string Join(this string[] obj)
        {
            return String.Join(",", obj);
        }

        private static string[] Split(this string obj)
        {
            return obj.Split(',');
        }
    }
}
