using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;

namespace TobascoV2.Extensions
{
    public static class XmlEntityExtensions
    {
        public static IEnumerable<XmlProperty> GetChilds(this XmlEntity entity)
        {
            return entity.Properties.Where(x => x.PropertyType.Name == Enums.Datatype.Child
                                             || x.PropertyType.Name == Enums.Datatype.ReadonlyChild);
        }

        public static IEnumerable<XmlProperty> GetChildCollections(this XmlEntity entity)
        {
            return entity.Properties.Where(x => x.PropertyType.Name == Enums.Datatype.ChildCollection);
        }
    }
}
