using System.Linq;
using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding
{
    internal class DapperEntityScaffolder : ScaffolderClassBase
    {
        internal override void AddFields(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            var lastProperty = entityContext.Properties.Last();
            entityContext.Properties.ForEach(property =>
            {
                Builder.AppendLine($"private {property.PropertyType.Name.GetDescription()} {property.GetFieldName()}");
                if (property.Equals(lastProperty))
                {
                    Builder.AppendLine(string.Empty);
                }
            });
        }

        internal override void AddMethods(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            Builder.StartMethod("public override", "ExpandoObject", "ToAnonymous");
            Builder.AddMethodBody("dynamic anymonous = base.ToAnonymous();");
            entityContext.Properties.ForEach(property =>
            {
                Builder.AddMethodBody($"anymonous.{property.Name} = {property.Name};");
            });
            Builder.AddMethodBody("return anymonous");
            Builder.EndMethod();
            Builder.AppendLine(string.Empty);
        }

        internal override void AddProperties(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            var lastProperty = entityContext.Properties.Last();
            entityContext.Properties.ForEach(property =>
            {
                if (tobascoContext.AddBusinessRules)
                {
                    Builder.StartPropertyWithBusinessRules("public", property);
                    Builder.AddPropertyBody($"get {{ return {property.GetFieldName()}; }}");
                    Builder.AddPropertyBody($"set {{ SetField(ref {property.GetFieldName()}, value, nameof({property.Name})); }}");
                    Builder.EndProperty();
                }
                else
                {
                    Builder.StartProperty("public", property);
                    Builder.EndProperty();
                }
                if (!property.Equals(lastProperty))
                {
                    Builder.AppendLine(string.Empty);
                }
            });
        }
    }
}//5ACaqwqu
