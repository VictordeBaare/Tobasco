using System;
using System.IO;
using TobascoV2.Builder;
using TobascoV2.Context;
using TobascoV2.Extensions;
using TobascoV2.Scaffolding.Helpers;

namespace TobascoV2.Scaffolding
{
    public abstract class ScaffolderClassBase : ScaffolderBase<ClassStringBuilder>
    {
        public ScaffolderClassBase() : base(new ClassStringBuilder()) { }

        public override void Scaffold(XmlEntity entityContext, ITobascoContext tobascoContext, string appRoot)
        {
            AddUsingNamespaces(entityContext, tobascoContext);

            Builder.StartNamesspace(tobascoContext.EntityContext.EntityLocation.GetNamespace());

            AddClass(entityContext, tobascoContext);

            Builder.EndNamespace();

            CreateOrOverwriteFile(
                $"{appRoot}//" +
                $"{FileLocationHelper.GetFileLocation(entityContext.EntityLocation, tobascoContext.EntityContext.EntityLocation)}//" +
                $"{entityContext.Name}.cs");
        }

        internal virtual void AddUsingNamespaces(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            tobascoContext.EntityContext.Namespaces.ForEach(x => Builder.AddUsing(x));
            entityContext.Namespaces.ForEach(x => Builder.AddUsing(x));
        }

        internal virtual void AddClass(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            string baseClass = GetBaseClass(entityContext, tobascoContext);
            if (entityContext.IsAbstract)
            {
                if (string.IsNullOrEmpty(baseClass))
                {
                    Builder.StartAbstractClass(entityContext.Name);
                }
                else
                {
                    Builder.StartAbstractClass(entityContext.Name, baseClass);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(baseClass))
                {
                    Builder.StartClass(entityContext.Name);
                }
                else
                {
                    Builder.StartClass(entityContext.Name, baseClass);
                }
            }

            AddFields(entityContext, tobascoContext);

            AddConstructor(entityContext, tobascoContext);

            AddProperties(entityContext, tobascoContext);

            AddMethods(entityContext, tobascoContext);

            Builder.EndClass();
        }

        private static string GetBaseClass(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            if(string.IsNullOrEmpty(entityContext.BaseClass) && string.IsNullOrEmpty(tobascoContext.BaseClass))
            {
                return string.Empty;
            }
            else if (string.IsNullOrEmpty(entityContext.BaseClass))
            {
                return tobascoContext.BaseClass;
            }
            return entityContext.BaseClass;
        }

        internal virtual void AddConstructor(XmlEntity entityContext, ITobascoContext tobascoContext) { }

        internal virtual void AddFields(XmlEntity entityContext, ITobascoContext tobascoContext) { }

        internal virtual void AddProperties(XmlEntity entityContext, ITobascoContext tobascoContext) { }

        internal virtual void AddMethods(XmlEntity entityContext, ITobascoContext tobascoContext) { }
    }
}
