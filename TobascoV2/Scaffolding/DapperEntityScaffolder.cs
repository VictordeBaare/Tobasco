using System.Linq;
using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding
{
    internal class DapperEntityScaffolder : ScaffolderBase
    {
        internal override void AddFields(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            var lastProperty = entityContext.Properties.Last();
            entityContext.Properties.ForEach(property =>
            {
                ClassBuilder.AppendLine($"private {property.PropertyType.Name.GetDescription()} {property.GetFieldName()}");
                if (property.Equals(lastProperty))
                {
                    ClassBuilder.AppendLine(string.Empty);
                }
            });
        }

        internal override void AddMethods(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            ClassBuilder.StartMethod("public override", "ExpandoObject", "ToAnonymous");
            ClassBuilder.AddMethodBody("dynamic anymonous = base.ToAnonymous();");
            entityContext.Properties.ForEach(property =>
            {
                ClassBuilder.AddMethodBody($"anymonous.{property.Name} = {property.Name};");
            });
            ClassBuilder.AddMethodBody("return anymonous");
            ClassBuilder.EndMethod();
            ClassBuilder.AppendLine(string.Empty);
        }

        internal override void AddProperties(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            var lastProperty = entityContext.Properties.Last();
            entityContext.Properties.ForEach(property =>
            {
                if (tobascoContext.AddBusinessRules)
                {
                    ClassBuilder.StartPropertyWithBusinessRules("public", property);
                    ClassBuilder.AddPropertyBody($"get {{ return {property.GetFieldName()}; }}");
                    ClassBuilder.AddPropertyBody($"set {{ SetField(ref {property.GetFieldName()}, value, nameof({property.Name})); }}");
                    ClassBuilder.EndProperty();
                }
                else
                {
                    ClassBuilder.StartProperty("public", property);
                    ClassBuilder.EndProperty();
                }
                if (!property.Equals(lastProperty))
                {
                    ClassBuilder.AppendLine(string.Empty);
                }
            });
        }
    }
}//5ACaqwqu
