using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Extensions;
using Tobasco.FileBuilder;

namespace Tobasco.Model.Builders
{
    public class RepositoryBuilder
    {
        private readonly EntityHandler _entity;
        private string _getRepositoryName;
        private string _getRepositoryInterfaceName;
        private EntityInformation _information;
        private string _getRepositoryContructorParameters;


        public RepositoryBuilder(EntityHandler entity, EntityInformation information)
        {
            _entity = entity;
            _information = information;
        }

        private StringBuilder GetSaveImplementation(StringBuilder builder, int tabs)
        {
            foreach (var itemToSave in _entity.Entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.Child))
            {
                builder.AppendLineWithTabs($"{_entity.Entity.Name.ToLower()}.{itemToSave.Name} = _{_entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save({_entity.Entity.Name.ToLower()}.{itemToSave.Name});", tabs);
            }
            builder.AppendLineWithTabs($"{_entity.Entity.Name.ToLower()} = _genericRepository.Save({_entity.Entity.Name.ToLower()});", tabs);
            foreach (var itemToSave in _entity.Entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.ChildCollection))
            {
                builder.AppendLineWithTabs($"foreach(var toSaveItem in {_entity.Entity.Name.ToLower()}.{itemToSave.Name})", tabs);
                builder.AppendLineWithTabs("{", tabs);
                builder.AppendLineWithTabs($"toSaveItem.{_entity.GetChildReferenceProperty(itemToSave.DataType.Type, _entity.Entity.Name).Name} = {_entity.Entity.Name.ToLower()}.Id;", tabs + 1);
                builder.AppendLineWithTabs($"_{_entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save(toSaveItem);", tabs + 1);
                builder.AppendLineWithTabs("}", tabs);
            }
            return builder;
        }

        private StringBuilder GetTransaction(StringBuilder builder, Func<StringBuilder, int, StringBuilder> implementation)
        {
            var tabs = 3;
            var trans = _information.Repository.Transaction;
            if (trans != null && trans.UseTransaction)
            {
                if (trans.InterfaceImplementation != null)
                {
                    builder.AppendLineWithTabs("_deadlockRetryHelper.ExecuteWithRetry(() => ", tabs);
                    builder.AppendLineWithTabs("{", tabs);
                    implementation(builder, tabs + 1);
                    builder.AppendLineWithTabs("});", tabs);
                }
                else
                {
                    builder.AppendLineWithTabs("using (var ts = new TransactionScope())", tabs);
                    builder.AppendLineWithTabs("{", tabs);
                    implementation(builder, tabs + 1);
                    builder.AppendLineWithTabs("    ts.Complete();", tabs);
                    builder.AppendLineWithTabs("}", tabs);
                }
                return builder;
            }
            return implementation(builder, tabs);
        }

        public string GetRepositoryName
        {
            get
            {
                if (string.IsNullOrEmpty(_getRepositoryName))
                {
                    _getRepositoryName = $"{_entity.Entity.Name}Repository";
                }
                return _getRepositoryName;
            }
        }

        public string GetRepositoryInterfaceName
        {
            get
            {
                if (string.IsNullOrEmpty(_getRepositoryInterfaceName))
                {
                    _getRepositoryInterfaceName = $"I{_entity.Entity.Name}Repository";
                }
                return _getRepositoryInterfaceName;
            }
        }

        private IEnumerable<string> GetRepositoryNamespaces
        {
            get
            {
                var listNamespaces = new List<string>();
                listNamespaces.AddRange(_information.Repository.Namespaces.Select(valueElement => valueElement.Value));
                listNamespaces.Add(_entity.GetEntityLocationOnId(_entity.GetRepository.EntityId).GetProjectLocation, s => !listNamespaces.Contains(s));
                foreach (var childRep in _entity.Entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.Child || x.DataType.Datatype == Enums.Datatype.ChildCollection))
                {
                    var projectLocation = _entity.GetProjectLocation(childRep.DataType.Type, _entity.GetRepository.EntityId);
                    if(listNamespaces.FirstOrDefault(x => x == projectLocation) == null)
                    {
                        listNamespaces.Add(projectLocation);
                    }
                }

                return listNamespaces;
            }
        }

        private string GetSaveMethod()
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs($"public {_entity.Entity.Name} Save({_entity.Entity.Name} {_entity.Entity.Name.ToLower()})", 0);
            builder.AppendLineWithTabs("{", 2);
            GetTransaction(builder, GetSaveImplementation);
            builder.AppendLineWithTabs($"return {_entity.Entity.Name.ToLower()};", 3);
            builder.AppendLineWithTabs("}", 2);
            return builder.ToString();
        }

        public IEnumerable<FileBuilder.OutputFile> Build(DynamicTextTransformation2 textTransformation)
        {
            textTransformation.WriteLine($"// Build {GetRepositoryName} for {_entity.GetRepository.GetProjectLocation}");
            var classFile = (ClassFile)FileManager.StartNewFile(GetRepositoryName, _entity.GetRepository.Project, _entity.GetRepository.Folder, Enums.FileType.Class);

            classFile.Namespaces.AddRange(GetRepositoryNamespaces);
            classFile.Namespaces.Add(_information.Repository.InterfaceLocation.GetProjectLocation);
            classFile.OwnNamespace = _information.Repository.GetNamespace;
            classFile.Constructor.ParameterWithField.AddRange(GetFieldWithParameters());
            classFile.BaseClass = $": {GetRepositoryInterfaceName}";
            classFile.Methods.Add(GetSaveMethod());

            textTransformation.WriteLine($"// Build {GetRepositoryInterfaceName} for {_entity.GetRepository.InterfaceLocation.GetProjectLocation}");
            var interfaceFile = (InterfaceFile)FileManager.StartNewFile(GetRepositoryInterfaceName, _entity.GetRepository.InterfaceLocation.Project, _entity.GetRepository.InterfaceLocation.Folder, Enums.FileType.Interface);
            interfaceFile.Namespaces.Add(_information.Repository.InterfaceLocation.GetProjectLocation);
            interfaceFile.Namespaces.AddRange(GetRepositoryNamespaces);
            interfaceFile.OwnNamespace = _information.Repository.InterfaceLocation.GetNamespace;        
            interfaceFile.Methods.Add($"{_entity.Entity.Name} Save({_entity.Entity.Name} {_entity.Entity.Name.ToLower()});");

            return new List<FileBuilder.OutputFile> { classFile, interfaceFile };
        }

        private IEnumerable<FieldWithParameter> GetFieldWithParameters()
        {
            List<FieldWithParameter> fields = new List<FieldWithParameter>();
            fields.Add(new FieldWithParameter { Field = "_genericRepository", Name = "genericRepository", Type = $"GenericRepository<{_entity.Entity.Name}>" });
            foreach (var childRep in _entity.SelectChildRepositoryInterfaces())
            {
                fields.Add(new FieldWithParameter { Field = $"_{childRep.FirstCharToLower()}", Name = childRep.FirstCharToLower(), Type = childRep });
            }
            return fields;
        }
    }
}
