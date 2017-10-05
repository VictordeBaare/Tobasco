using System.Collections.Generic;
using System.Linq;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
    public class DefaultGenericRepositoryBuilder : GenericRepositoryBuilderBase
    {
        public DefaultGenericRepositoryBuilder(EntityInformation information) : base(information) { }

        protected virtual string SaveMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositorySave);
            return template.GetText;
        }

        protected virtual string ListSaveMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryBulkSave);
            return template.GetText;
        }

        protected virtual string InsertMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryInsert);
            return template.GetText;
        }

        protected virtual string DeleteMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryDelete);
            return template.GetText;
        }

        protected virtual string GetByIdMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryGetById);
            return template.GetText;
        }

        protected virtual string UpdateMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryUpdate);
            return template.GetText;
        }

        protected virtual string ToAnonymousMethod()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryToAnonymous);
            return template.GetText;
        }

        protected virtual string QueryMutlipleT()
        {
            var template = new Template();
            template.SetTemplate(Resources.GenericRepositoryQueryMultipleT);
            return template.GetText;
        }

        protected virtual ClassFile BuildClassFile()
        {
            var classFile = FileManager.StartNewClassFile(GetGenericRepositoryName, Classlocation.Project, Classlocation.Folder);
            classFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", "Dapper", "System.Data", Interfacelocation.GetProjectLocation, "System.Linq", "System.Collections.Generic", "static Dapper.SqlMapper" });
            classFile.Namespaces.AddRange(Information.Repository.Namespaces.Select(x => x.Value).Concat(Information.GenericRepository.Namespaces.Select(x => x.Value)));
            classFile.Namespaces.Add("System.Globalization");
            classFile.OwnNamespace = Information.Repository.FileLocation.GetNamespace;
            classFile.NameExtension = "<T>";
            classFile.BaseClass = ": IGenericRepository<T> where T : EntityBase, new()";
            classFile.Constructor.Parameters.Add(new TypeWithName("connectionFactory", "IConnectionFactory"));
            classFile.Constructor.CustomImplementation.Add("ConnectionFactory = connectionFactory;");
            classFile.Properties.Add("public IConnectionFactory ConnectionFactory { get; }");
            classFile.Methods.Add(SaveMethod());
            classFile.Methods.Add(InsertMethod());
            classFile.Methods.Add(UpdateMethod());
            classFile.Methods.Add(DeleteMethod());
            classFile.Methods.Add(ListSaveMethod());
            classFile.Methods.Add(GetByIdMethod());
            classFile.Methods.Add(ToAnonymousMethod());
            classFile.Methods.Add(QueryMutlipleT());
            return classFile;
        }

        protected virtual InterfaceFile BuildInterfaceFile()
        {
            var interfaceFile = FileManager.StartNewInterfaceFile(GetInterfaceGenericRepositoryName, Interfacelocation.Project, Interfacelocation.Folder);
            interfaceFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", Interfacelocation.GetProjectLocation, "System.Collections.Generic", "Dapper", "static Dapper.SqlMapper" });
            interfaceFile.Namespaces.AddRange(Information.Repository.Namespaces.Select(x => x.Value).Concat(Information.GenericRepository.Namespaces.Select(x => x.Value)));
            interfaceFile.OwnNamespace = Interfacelocation.GetNamespace;
            interfaceFile.NameExtension = "<T> where T : EntityBase, new()";
            interfaceFile.Properties.Add("IConnectionFactory ConnectionFactory { get; }");
            interfaceFile.Methods.Add("T Save(T entity);");
            interfaceFile.Methods.Add("T GetById(long id); ");
            interfaceFile.Methods.Add("IEnumerable<T> Save(IEnumerable<T> entities);");
            interfaceFile.Methods.Add("IEnumerable<T> QueryMultiple(string StoredProcedure, DynamicParameters parameters, Func<GridReader, IEnumerable<T>> readerFunc);");

            return interfaceFile;
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            return new List<FileBuilder.OutputFile> { BuildClassFile(), BuildInterfaceFile() };
        }
    }
}
