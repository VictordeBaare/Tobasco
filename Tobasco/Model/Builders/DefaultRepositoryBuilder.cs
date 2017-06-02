using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;

namespace Tobasco.Model.Builders
{
    public class DefaultRepositoryBuilder : RepositoryBuilderBase
    {

        public DefaultRepositoryBuilder(EntityHandler entity, EntityInformation information) : base(entity, information)
        {
        }
        
        private StringBuilder GetTransaction(StringBuilder builder, Func<StringBuilder, int, StringBuilder> implementation)
        {
            var tabs = 3;
            var trans = Information.Repository.Transaction;
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
        
        private string GetSaveMethod()
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs($"public {Entity.Entity.Name} Save({Entity.Entity.Name} {Entity.Entity.Name.ToLower()})", 0);
            builder.AppendLineWithTabs("{", 2);
            GetTransaction(builder, GetSaveImplementation);
            builder.AppendLineWithTabs($"return {Entity.Entity.Name.ToLower()};", 3);
            builder.AppendLineWithTabs("}", 2);
            return builder.ToString();
        }

        public override IEnumerable<FileBuilder.OutputFile> Build(DynamicTextTransformation2 textTransformation)
        {
            textTransformation.WriteLine($"// Build {GetRepositoryName} for {Entity.GetRepository.GetProjectLocation}");
            var classFile = (ClassFile)FileManager.StartNewFile(GetRepositoryName, Entity.GetRepository.Project, Entity.GetRepository.Folder, Enums.FileType.Class);

            classFile.Namespaces.AddRange(GetRepositoryNamespaces);
            classFile.Namespaces.Add(Information.Repository.InterfaceLocation.GetProjectLocation);
            classFile.OwnNamespace = Information.Repository.GetNamespace;
            classFile.Constructor.ParameterWithField.AddRange(GetFieldWithParameters());
            classFile.BaseClass = $": {GetRepositoryInterfaceName}";
            classFile.Methods.Add(GetSaveMethod());

            textTransformation.WriteLine($"// Build {GetRepositoryInterfaceName} for {Entity.GetRepository.InterfaceLocation.GetProjectLocation}");
            var interfaceFile = (InterfaceFile)FileManager.StartNewFile(GetRepositoryInterfaceName, Entity.GetRepository.InterfaceLocation.Project, Entity.GetRepository.InterfaceLocation.Folder, Enums.FileType.Interface);
            interfaceFile.Namespaces.Add(Information.Repository.InterfaceLocation.GetProjectLocation);
            interfaceFile.Namespaces.AddRange(GetRepositoryNamespaces);
            interfaceFile.OwnNamespace = Information.Repository.InterfaceLocation.GetNamespace;        
            interfaceFile.Methods.Add($"{Entity.Entity.Name} Save({Entity.Entity.Name} {Entity.Entity.Name.ToLower()});");

            return new List<FileBuilder.OutputFile> { classFile, interfaceFile };
        }

        private IEnumerable<FieldWithParameter> GetFieldWithParameters()
        {
            List<FieldWithParameter> fields = new List<FieldWithParameter>
            {
                new FieldWithParameter
                {
                    Field = "_genericRepository",
                    Name = "genericRepository",
                    Type = $"GenericRepository<{Entity.Entity.Name}>"
                }
            };
            fields.AddRange(Entity.SelectChildRepositoryInterfaces().Select(childRep => new FieldWithParameter {Field = $"_{childRep.FirstCharToLower()}", Name = childRep.FirstCharToLower(), Type = childRep}));
            return fields;
        }

        private StringBuilder GetSaveImplementation(StringBuilder builder, int tabs)
        {
            foreach (var itemToSave in Entity.Entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.Child))
            {
                builder.AppendLineWithTabs($"{Entity.Entity.Name.ToLower()}.{itemToSave.Name} = _{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save({Entity.Entity.Name.ToLower()}.{itemToSave.Name});", tabs);
            }
            builder.AppendLineWithTabs($"{Entity.Entity.Name.ToLower()} = _genericRepository.Save({Entity.Entity.Name.ToLower()});", tabs);
            foreach (var itemToSave in Entity.Entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.ChildCollection))
            {
                builder.AppendLineWithTabs($"foreach(var toSaveItem in {Entity.Entity.Name.ToLower()}.{itemToSave.Name})", tabs);
                builder.AppendLineWithTabs("{", tabs);
                builder.AppendLineWithTabs($"toSaveItem.{Entity.GetChildReferenceProperty(itemToSave.DataType.Type, Entity.Entity.Name).Name} = {Entity.Entity.Name.ToLower()}.Id;", tabs + 1);
                builder.AppendLineWithTabs($"_{Entity.GetRepositoryInterface(itemToSave.DataType.Type).FirstCharToLower()}.Save(toSaveItem);", tabs + 1);
                builder.AppendLineWithTabs("}", tabs);
            }
            return builder;
        }

    }
}
