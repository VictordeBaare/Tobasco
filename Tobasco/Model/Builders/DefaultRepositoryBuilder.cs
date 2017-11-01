using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.Builders.RepositoryBuilders;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
    public class DefaultRepositoryBuilder : RepositoryBuilderBase
    {
        private GetFullEntityByIdBuilder _getFullEntityByIdBuilder;
        private GetFullEntityByIdReader _getFullEntityByIdReaderBuilder;

        public DefaultRepositoryBuilder(EntityHandler entity, Repository repository) : base(entity, repository)
        {
            _getFullEntityByIdBuilder = new GetFullEntityByIdBuilder(entity, repository);
            _getFullEntityByIdReaderBuilder = new GetFullEntityByIdReader(entity, repository);
        }        

        protected virtual string GetSaveMethod()
        {
            var template = new Template();
            var trans = Repository.Transaction;
            template.SetTemplate(trans != null && trans.UseTransaction ? GetSaveWithTransactionTemplate : GetSaveWithoutTransactionTemplate);
            template.Fill(GetSaveParameters());
            return template.GetText;
        }

        protected virtual string GetSaveWithTransactionTemplate => RepositoryResources.RepositorySaveWithTransaction;

        protected virtual string GetSaveWithoutTransactionTemplate => RepositoryResources.RepositorySave;
        
        protected TemplateParameter GetSaveParameters()
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
            classFile.Namespaces.Add("Dapper");
            classFile.Namespaces.Add("System.Linq");
            classFile.Namespaces.Add("static Dapper.SqlMapper");
            classFile.OwnNamespace = Repository.FileLocation.GetNamespace;
            AddFieldsWithParameterToConstructor(classFile);
            classFile.BaseClass = $": {GetRepositoryInterfaceName}";
            classFile.Methods.Add(GetSaveMethod());
            classFile.Methods.Add(GetByIdMethod());
            if (Entity.GetDatabase.StoredProcedures.GenerateGetById.Generate)
            {
                classFile.Methods.Add(_getFullEntityByIdBuilder.Build());
                classFile.Methods.Add(_getFullEntityByIdReaderBuilder.Build());
            }
            return classFile;
        }

        protected virtual InterfaceFile GetInterfaceFile()
        {
            var interfaceFile = FileManager.StartNewInterfaceFile(GetRepositoryInterfaceName, Repository.InterfaceLocation.Project, Repository.InterfaceLocation.Folder);
            interfaceFile.Namespaces.Add(Repository.InterfaceLocation.GetProjectLocation);
            interfaceFile.Namespaces.Add("static Dapper.SqlMapper");
            interfaceFile.Namespaces.Add("Dapper");
            interfaceFile.Namespaces.Add("System.Linq");
            interfaceFile.Namespaces.AddRange(GetRepositoryNamespaces);
            interfaceFile.OwnNamespace = Repository.InterfaceLocation.GetNamespace;
            interfaceFile.Methods.Add($"{GetEntityName} Save({GetEntityName} {GetEntityName.ToLower()});");
            interfaceFile.Methods.Add($"{GetEntityName} GetById(long id);");
            if (Entity.GetDatabase.StoredProcedures.GenerateGetById.Generate)
            {
                interfaceFile.Methods.Add($"{GetEntityName} GetFullObjectById(long id);");
            }
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

        protected virtual void AddFieldsWithParameterToConstructor(ClassFile classFile)
        {
            foreach (var item in Entity.SelectChildRepositoryInterfaces(Entity.Entity.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection || x.DataType.Datatype == Datatype.Child))){
                classFile.Constructor.AddFieldWithParameter(new Field("private", $"_{item.FirstCharToLower()}", item), new TypeWithName(item.FirstCharToLower(), item));
            }
            AddGenericRepositoryParameter(classFile);
        }

        protected virtual void AddGenericRepositoryParameter(ClassFile classFile)
        {
            classFile.Constructor.AddFieldWithParameter(new Field("private", "_genericRepository", $"IGenericRepository<{GetEntityName}>"), new TypeWithName("genericRepository", $"IGenericRepository<{GetEntityName}>"));
        }
    }
}
