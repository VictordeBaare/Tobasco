using System;
using System.IO;
using TobascoV2.Builder;
using TobascoV2.Context;
using TobascoV2.Extensions;
using TobascoV2.Scaffolding.Helpers;

namespace TobascoV2.Scaffolding
{
    public abstract class ScaffolderBase 
    {
        public ScaffolderBase()
        {
            ClassBuilder = new ClassStringBuilder();
        }

        protected ClassStringBuilder ClassBuilder { get; }

        protected virtual void Scaffold(EntityContext entityContext, ITobascoContext tobascoContext, string appRoot)
        {
            AddUsingNamespaces(entityContext, tobascoContext);

            ClassBuilder.StartNamesspace(tobascoContext.EntityContext.EntityLocation.GetNamespace());

            AddClass(entityContext, tobascoContext);

            ClassBuilder.EndNamespace();

            CreateOrOverwriteFile(
                $"{appRoot}//" +
                $"{FileLocationHelper.GetFileLocation(entityContext.EntityLocation, tobascoContext.EntityContext.EntityLocation)}//" +
                $"{entityContext.Name}.cs");
        }

        protected void CreateOrOverwriteFile(string path)
        {
            File.WriteAllText(path, ClassBuilder.ToString());
        }

        internal virtual void AddUsingNamespaces(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            tobascoContext.EntityContext.Namespaces.ForEach(x => ClassBuilder.AddUsing(x));
            entityContext.Namespaces.ForEach(x => ClassBuilder.AddUsing(x));
        }

        internal virtual void AddClass(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            string baseClass = GetBaseClass(entityContext, tobascoContext);
            if (entityContext.IsAbstract)
            {
                if (string.IsNullOrEmpty(baseClass))
                {
                    ClassBuilder.StartAbstractClass(entityContext.Name);
                }
                else
                {
                    ClassBuilder.StartAbstractClass(entityContext.Name, baseClass);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(baseClass))
                {
                    ClassBuilder.StartClass(entityContext.Name);
                }
                else
                {
                    ClassBuilder.StartClass(entityContext.Name, baseClass);
                }
            }

            AddFields(entityContext, tobascoContext);

            AddConstructor(entityContext, tobascoContext);

            AddProperties(entityContext, tobascoContext);

            AddMethods(entityContext, tobascoContext);

            ClassBuilder.EndClass();
        }

        private static string GetBaseClass(EntityContext entityContext, ITobascoContext tobascoContext)
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

        internal virtual void AddConstructor(EntityContext entityContext, ITobascoContext tobascoContext) { }

        internal virtual void AddFields(EntityContext entityContext, ITobascoContext tobascoContext) { }

        internal virtual void AddProperties(EntityContext entityContext, ITobascoContext tobascoContext) { }

        internal virtual void AddMethods(EntityContext entityContext, ITobascoContext tobascoContext) { }
    }
}
