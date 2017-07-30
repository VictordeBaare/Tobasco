﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
    public class DefaultRepositoryBuilder : RepositoryBuilderBase
    {
        public DefaultRepositoryBuilder(EntityHandler entity, Repository repository) : base(entity, repository)
        {
        }        

        protected virtual string GetSaveMethod()
        {
            var template = new Template();
            var trans = Repository.Transaction;
            template.SetTemplate(trans != null && trans.UseTransaction ? Resources.RepositorySaveWithTransaction : Resources.RepositorySave);
            template.Fill(GetSaveParameters());
            return template.GetText;
        }

        private TemplateParameter GetSaveParameters()
        {
            var parameters = new TemplateParameter();

            parameters.Add(RepositoryBuilderConstants.SaveChild, GetSaveableChilds());
            parameters.Add(RepositoryBuilderConstants.SaveChildCollections, GetSaveableChildCollection());
            parameters.Add(RepositoryBuilderConstants.EntityName, GetEntityName);
            parameters.Add(RepositoryBuilderConstants.EntityNameLowerCase, GetEntityName.ToLower());

            return parameters;
        }

        private List<string> GetSaveableChilds()
        {
            var list = new List<string>();
            foreach (var itemToSave in GetChildProperties)
            {
                if (itemToSave.Required)
                {
                    list.Add($"{GetEntityName.ToLower()}.{itemToSave.Name} = _{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save({GetEntityName.ToLower()}.{itemToSave.Name});");
                }
                else
                {
                    var builder = new StringBuilder();
                    builder.AppendLine($"if ({GetEntityName.ToLower()}.{itemToSave.Name} != null)");
                    builder.AppendLine("{");
                    builder.AppendLine($"{GetEntityName.ToLower()}.{itemToSave.Name} = _{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save({GetEntityName.ToLower()}.{itemToSave.Name});");
                    builder.AppendLine("}");
                    list.Add(builder.ToString());
                }
            }
            return list;
        }

        private List<string> GetSaveableChildCollection()
        {
            var list = new List<string>();

            foreach (var itemToSave in GetChildCollectionProperties)
            {
                var builder = new StringBuilder();
                builder.AppendLineWithTabs($"foreach(var toSaveItem in {GetEntityName.ToLower()}.{itemToSave.Name})", 0);
                builder.AppendLineWithTabs("{", 0);
                builder.AppendLineWithTabs($"toSaveItem.{Entity.GetChildReferenceProperty(itemToSave.DataType.Type, GetEntityName).Name} = {GetEntityName.ToLower()}.Id;", 1);
                builder.AppendLineWithTabs($"_{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save(toSaveItem);", 1);
                builder.AppendLineWithTabs("}", 0);
                list.Add(builder.ToString());
            }
            
            return list;
        }

        protected virtual string GetByIdMethod()
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs($"public {GetEntityName} GetById(long id)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("return _genericRepository.GetById(id);", 3);
            builder.AppendLineWithTabs("}", 2);
            return builder.ToString();
        }

        protected virtual ClassFile GetClassFile()
        {
            var classFile = FileManager.StartNewClassFile(GetRepositoryName, Repository.FileLocation.Project, Repository.FileLocation.Folder);
            classFile.Namespaces.AddRange(GetRepositoryNamespaces);
            classFile.Namespaces.Add(Repository.InterfaceLocation.GetProjectLocation);
            classFile.Namespaces.Add("System.Collections.Generic");
            classFile.OwnNamespace = Repository.FileLocation.GetNamespace;
            AddFieldsWithParameterToConstructor(classFile);
            classFile.BaseClass = $": {GetRepositoryInterfaceName}";
            classFile.Methods.Add(GetSaveMethod());
            classFile.Methods.Add(GetByIdMethod());
            return classFile;
        }

        protected virtual InterfaceFile GetInterfaceFile()
        {
            var interfaceFile = FileManager.StartNewInterfaceFile(GetRepositoryInterfaceName, Repository.InterfaceLocation.Project, Repository.InterfaceLocation.Folder);
            interfaceFile.Namespaces.Add(Repository.InterfaceLocation.GetProjectLocation);
            interfaceFile.Namespaces.AddRange(GetRepositoryNamespaces);
            interfaceFile.OwnNamespace = Repository.InterfaceLocation.GetNamespace;
            interfaceFile.Methods.Add($"{GetEntityName} Save({GetEntityName} {GetEntityName.ToLower()});");
            interfaceFile.Methods.Add($"{GetEntityName} GetById(long id);");
            return interfaceFile;
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            OutputPaneManager.WriteToOutputPane($"Build {GetRepositoryName} for {Entity.GetRepository.FileLocation.GetProjectLocation}");
            var classFile = GetClassFile();

            OutputPaneManager.WriteToOutputPane($"Build {GetRepositoryInterfaceName} for {Entity.GetRepository.InterfaceLocation.GetProjectLocation}");
            var interfaceFile = GetInterfaceFile();

            return new List<FileBuilder.OutputFile> { classFile, interfaceFile };
        }

        private void AddFieldsWithParameterToConstructor(ClassFile classFile)
        {
            foreach (var item in Entity.SelectChildRepositoryInterfaces(Entity.Entity.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection || x.DataType.Datatype == Datatype.Child))){
                classFile.Constructor.AddFieldWithParameter(GetField("private", item, $"_{item.FirstCharToLower()}"), GetParameter(item, item.FirstCharToLower()));
            }
            classFile.Constructor.AddFieldWithParameter(GetField("private", $"IGenericRepository<{GetEntityName}>", "_genericRepository"), GetParameter($"IGenericRepository<{GetEntityName}>", "genericRepository"));
        }

        private Field GetField(string modifier, string type, string name)
        {
            return new Field
            {
                Modifier = modifier,
                Type = type,
                Name = name
            };
        }

        private TypeWithName GetParameter(string type, string name)
        {
            return new TypeWithName
            {
                Type = type,
                Name = name
            };
        }
    }
}
