using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Security;

namespace Tobasco.Model.Builders
{
    public class DefaultSecurityRepositoryBuilder : SecurityRepositoryBase
    {
        public DefaultSecurityRepositoryBuilder(EntityHandler entity, Dac dac) : base(entity, dac)
        {
        }

        public override string GetEntityName => $"{base.GetEntityName}Dac";

        protected override IEnumerable<Property> GetChildProperties
        {
            get { return Dac.Properties.Where(x => x.DataType.Datatype == Datatype.Child); }
        }

        protected override IEnumerable<Property> GetChildCollectionProperties
        {
            get { return Dac.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection); }
        }

        protected override IEnumerable<string> GetRepositoryNamespaces
        {
            get
            {
                var listNamespaces = new List<string>();
                listNamespaces.AddRange(Dac.Repository.Namespaces.Select(valueElement => valueElement.Value));
                foreach (var childRep in GetChildProperties.Concat(GetChildCollectionProperties))
                {
                    var projectLocation = Entity.GetProjectLocation(childRep.DataType.Type, Dac.Repository.EntityId);
                    if (listNamespaces.FirstOrDefault(x => x == projectLocation) == null)
                    {
                        listNamespaces.Add(projectLocation);
                    }
                }

                return listNamespaces;
            }
        }

        protected virtual StringBuilder GetTransaction(StringBuilder builder, Func<StringBuilder, int, StringBuilder> implementation)
        {
            var tabs = 3;
            var trans = Dac.Repository.Transaction;
            if (trans != null && trans.UseTransaction)
            {
                builder.AppendLineWithTabs("using (var ts = new TransactionScope())", tabs);
                builder.AppendLineWithTabs("{", tabs);
                implementation(builder, tabs + 1);
                builder.AppendLineWithTabs("    ts.Complete();", tabs);
                builder.AppendLineWithTabs("}", tabs);
            }
            return implementation(builder, tabs);
        }

        protected virtual string GetSaveMethod()
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs($"public {GetEntityName} Save({GetEntityName} {GetEntityName.ToLower()})", 0);
            builder.AppendLineWithTabs("{", 2);
            GetTransaction(builder, GetSaveImplementation);
            builder.AppendLineWithTabs($"return {GetEntityName.ToLower()};", 3);
            builder.AppendLineWithTabs("}", 2);
            return builder.ToString();
        }

        protected virtual ClassFile GetClassFile()
        {
            var classFile = FileManager.StartNewClassFile(GetRepositoryName, Dac.Repository.FileLocation.Project, Dac.Repository.FileLocation.Folder);
            classFile.Namespaces.AddRange(GetRepositoryNamespaces);
            classFile.Namespaces.Add(Dac.FileLocation.GetProjectLocation, s => !classFile.Namespaces.Contains(s));
            classFile.Namespaces.Add(Dac.Repository.InterfaceLocation.GetProjectLocation, s => !classFile.Namespaces.Contains(s));
            classFile.OwnNamespace = Dac.Repository.FileLocation.GetNamespace;
            classFile.Constructor.ParameterWithField.AddRange(GetFieldWithParameters());
            classFile.BaseClass = $": {GetRepositoryInterfaceName}";
            classFile.Methods.Add(GetSaveMethod());
            return classFile;
        }

        protected virtual InterfaceFile GetInterfaceFile()
        {
            var interfaceFile = FileManager.StartNewInterfaceFile(GetRepositoryInterfaceName, Dac.Repository.InterfaceLocation.Project, Dac.Repository.InterfaceLocation.Folder);
            interfaceFile.Namespaces.AddRange(GetRepositoryNamespaces);
            interfaceFile.Namespaces.Add(Dac.Repository.InterfaceLocation.GetProjectLocation, s => !interfaceFile.Namespaces.Contains(s));
            interfaceFile.Namespaces.Add(Dac.FileLocation.GetProjectLocation, s => !interfaceFile.Namespaces.Contains(s));
            interfaceFile.OwnNamespace = Dac.Repository.InterfaceLocation.GetNamespace;
            interfaceFile.Methods.Add($"{GetEntityName} Save({GetEntityName} {GetEntityName.ToLower()});");
            return interfaceFile;
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            OutputPaneManager.WriteToOutputPane($"Build {GetRepositoryName} for {Dac.Repository.FileLocation.GetProjectLocation}");
            var classFile = GetClassFile();

            OutputPaneManager.WriteToOutputPane($"Build {GetRepositoryInterfaceName} for {Dac.Repository.InterfaceLocation.GetProjectLocation}");
            var interfaceFile = GetInterfaceFile();

            return new List<FileBuilder.OutputFile> { classFile, interfaceFile };
        }

        private IEnumerable<FieldWithParameter> GetFieldWithParameters()
        {
            List<FieldWithParameter> fields = new List<FieldWithParameter>
            {
                new FieldWithParameter
                {
                    Field = new TypeWithName {Name = "_genericRepository", Type = $"GenericRepository<{GetEntityName}>"},
                    Parameter = new TypeWithName { Name = "genericRepository",Type = $"GenericRepository<{GetEntityName}>"}
                }
            };
            fields.AddRange(Entity.SelectChildRepositoryInterfaces(Dac.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection || x.DataType.Datatype == Datatype.Child)).Select(childRep => new FieldWithParameter { Field = new TypeWithName {Name = $"_{childRep.FirstCharToLower()}", Type = childRep }, Parameter = new TypeWithName { Name = childRep.FirstCharToLower(), Type = childRep }}));
            return fields;
        }

        protected virtual StringBuilder GetSaveImplementation(StringBuilder builder, int tabs)
        {
            foreach (var itemToSave in GetChildProperties)
            {
                builder.AppendLineWithTabs($"{GetEntityName.ToLower()}.{itemToSave.Name} = _{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save({Entity.Entity.Name.ToLower()}.{itemToSave.Name});", tabs);
            }
            builder.AppendLineWithTabs($"{GetEntityName.ToLower()} = _genericRepository.Save({GetEntityName.ToLower()});", tabs);
            foreach (var itemToSave in GetChildCollectionProperties)
            {
                builder.AppendLineWithTabs($"foreach(var toSaveItem in {GetEntityName.ToLower()}.{itemToSave.Name})", tabs);
                builder.AppendLineWithTabs("{", tabs);
                builder.AppendLineWithTabs($"toSaveItem.{Entity.GetChildReferenceProperty(itemToSave.DataType.Type, GetEntityName).Name} = {GetEntityName.ToLower()}.Id;", tabs + 1);
                builder.AppendLineWithTabs($"_{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save(toSaveItem);", tabs + 1);
                builder.AppendLineWithTabs("}", tabs);
            }
            return builder;
        }


    }
}