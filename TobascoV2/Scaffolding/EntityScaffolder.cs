using System;
using System.Linq;
using TobascoV2.Context;
using TobascoV2.Extensions;
using TobascoV2.Scaffolding.Helpers;

namespace TobascoV2.Scaffolding
{
    internal class EntityScaffolder : ScaffolderClassBase
    {
        internal override void AddProperties(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            var lastProperty = entityContext.Properties.Last();
            entityContext.Properties.ForEach(property =>
            {
                if (tobascoContext.AddBusinessRules)
                {
                    Builder.AddPropertyWithBusinessRule("public", property);
                }
                else
                {
                    Builder.AddProperty("public", property);
                }
                if (!property.Equals(lastProperty))
                {
                    Builder.AppendLine(string.Empty);
                }
            });
        }
    }
}
