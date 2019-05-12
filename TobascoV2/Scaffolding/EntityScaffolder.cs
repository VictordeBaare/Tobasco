using System;
using System.Linq;
using TobascoV2.Context;
using TobascoV2.Extensions;
using TobascoV2.Scaffolding.Helpers;

namespace TobascoV2.Scaffolding
{
    internal class EntityScaffolder : ScaffolderBase
    {
        internal override void AddProperties(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            var lastProperty = entityContext.Properties.Last();
            entityContext.Properties.ForEach(property =>
            {
                if (tobascoContext.AddBusinessRules)
                {
                    ClassBuilder.AddPropertyWithBusinessRule("public", property);
                }
                else
                {
                    ClassBuilder.AddProperty("public", property);
                }
                if (!property.Equals(lastProperty))
                {
                    ClassBuilder.AppendLine(string.Empty);
                }
            });
        }
    }
}
