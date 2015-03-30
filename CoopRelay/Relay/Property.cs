using CoopRelay.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;

namespace CoopRelay.Relay
{
    public static class Property
    {
        public static CoopRelay.Domain.Models.Property Map(Umbraco.Core.Models.Property p)
        {
            return new CoopRelay.Domain.Models.Property()
            {
                Alias = p.Alias,
                Value = p.Value
            };
        }

        public static CoopRelay.Domain.Models.PropertyCollection Map(Umbraco.Core.Models.PropertyCollection pc)
        {
            var result = new CoopRelay.Domain.Models.PropertyCollection();
            foreach (var p in pc)
            {
                result.Add(Map(p));
            }
            return result;
        }

        public static CoopRelay.Domain.Models.Property Map(Umbraco.Core.Models.IPublishedProperty p)
        {
            return new CoopRelay.Domain.Models.Property()
            {
                Alias = p.PropertyTypeAlias,
                Value = p.Value
            };
        }

        public static CoopRelay.Domain.Models.PropertyCollection Map(ICollection<Umbraco.Core.Models.IPublishedProperty> pc)
        {
            var result = new CoopRelay.Domain.Models.PropertyCollection();
            foreach(var p in pc){
                result.Add(Map(p));
            }
            return result;
        }

        public static CoopRelay.Domain.Models.Property Map(umbraco.cms.businesslogic.property.Property p)
        {
            return new CoopRelay.Domain.Models.Property()
            {
                Alias = p.PropertyType.Alias,
                Value = p.Value
            };
        }

        public static CoopRelay.Domain.Models.PropertyCollection Map(umbraco.cms.businesslogic.property.Properties pc)
        {
            var result = new CoopRelay.Domain.Models.PropertyCollection();
            foreach (var p in pc)
            {
                result.Add(Map(p));
            }
            return result;
        }
    }
}
