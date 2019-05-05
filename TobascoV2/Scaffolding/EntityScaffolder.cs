using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding
{
    internal class EntityScaffolder : ScaffolderBase
    {
        internal void Scaffold(EntityContext entityContext, TobascoContext tobascoContext, string appRoot)
        {
            tobascoContext.EntityContext.Namespaces.ForEach(x => ClassBuilder.AddUsing(x));
            entityContext.Namespaces.ForEach(x => ClassBuilder.AddUsing(x));
            ClassBuilder.StartNamesspace(tobascoContext.EntityContext.EntityLocation.NameSpace);
            ClassBuilder.StartClass(entityContext.Name);

            entityContext.Properties.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.Description))
                {
                    ClassBuilder.AddProperty("public", x.PropertyType.GetDescription(), x.Name);
                }
                else
                {
                    ClassBuilder.AddProperty("public", x.PropertyType.GetDescription(), x.Name, x.Description);
                }
            });

            ClassBuilder.EndClass();
            ClassBuilder.EndNamespace();

            CreateOrOverwriteFile($"{appRoot}//{entityContext.GetNameWithPath()}");
        }
    }
}
