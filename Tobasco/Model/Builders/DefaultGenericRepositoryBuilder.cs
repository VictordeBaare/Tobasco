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
    public class DefaultGenericRepositoryBuilder : GenericRepositoryBuilderBase
    {
        public DefaultGenericRepositoryBuilder(EntityInformation information) : base(information) { }

        protected virtual string SaveMethod()
        {
            var builder = new StringBuilder();
            
            builder.AppendLineWithTabs("public T Save(T entity)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("if(entity.IsDeleted)", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("Delete(entity);", 4);
            builder.AppendLineWithTabs("return entity;", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("if(entity.IsNew)", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("return Insert(entity);", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("if(entity.IsDirty)", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("return Update(entity);", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("return entity;", 3);
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        protected virtual string ListSaveMethod()
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("public IEnumerable<T> Save(IEnumerable<T> entities)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("var enumerable = entities as IList<T> ?? entities.ToList();", 3);
            builder.AppendLineWithTabs("foreach (var entity in enumerable)", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("Save(entity);", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("return enumerable;", 3);
            builder.AppendLineWithTabs("}", 2);
            
            return builder.ToString();
        }

        protected virtual string InsertMethod()
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("public T Insert(T entity)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("using (var connection = ConnectionFactory.GetConnection())", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("connection.Query<long, byte[], T>($\"[dbo].[{typeof(T).Name}_Insert]\", (id, rowversion) =>", 4);
            builder.AppendLineWithTabs("{", 4);
            builder.AppendLineWithTabs("entity.Id = id;", 5);
            builder.AppendLineWithTabs("entity.RowVersion = rowversion;", 5);
            builder.AppendLineWithTabs("return entity;", 5);
            builder.AppendLineWithTabs("}, ToAnonymous(entity, false), splitOn: \"RowVersion\", commandType: CommandType.StoredProcedure);", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("entity.MarkOld();", 3);
            builder.AppendLineWithTabs("return entity;", 3);
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        protected virtual string DeleteMethod()
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("public void Delete(T entity)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("using (var connection = ConnectionFactory.GetConnection())", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("connection.Execute($\"[dbo].[{ typeof(T).Name}_Delete]\",", 4);
            builder.AppendLineWithTabs("new {entity.Id, entity.RowVersion, entity.ModifiedBy },", 5);
            builder.AppendLineWithTabs("commandType: CommandType.StoredProcedure);", 5);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        protected virtual string GetByIdMethod()
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("public T GetById(long id)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("T entity;", 3);
            builder.AppendLineWithTabs("using (var connection = ConnectionFactory.GetConnection())", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("entity = connection.QuerySingle<T>($\"[dbo].[{ typeof(T).Name}_GetById]\",", 4);
            builder.AppendLineWithTabs("new {Id = id},", 5);
            builder.AppendLineWithTabs("commandType: CommandType.StoredProcedure);", 5);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("entity.MarkOld();", 3);
            builder.AppendLineWithTabs("return entity;", 3);
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        protected virtual string UpdateMethod()
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("public T Update(T entity)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("using (var connection = ConnectionFactory.GetConnection())", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("try", 4);
            builder.AppendLineWithTabs("{", 4);
            builder.AppendLineWithTabs("entity.RowVersion = connection.ExecuteScalar<byte[]>($\"[dbo].[{ typeof(T).Name}_Update]\",", 5);
            builder.AppendLineWithTabs("ToAnonymous(entity, true),", 6);
            builder.AppendLineWithTabs("commandType: CommandType.StoredProcedure);", 6);
            builder.AppendLineWithTabs("}", 4);
            builder.AppendLineWithTabs("catch(SqlException ex)", 4);
            builder.AppendLineWithTabs("{", 4);
            builder.AppendLineWithTabs("if (ex.Number == 50000 && ex.Class == 16 && ex.Message.Contains(\" is al gewijzigd door een andere gebruiker.\"))", 5);
            builder.AppendLineWithTabs("{", 5);
            builder.AppendLineWithTabs("throw new DBConcurrencyException($\"Dirty write detected while updating { typeof(T).Name} with id { entity.Id} at { DateTime.Now.ToString(CultureInfo.InvariantCulture)}\");", 6);
            builder.AppendLineWithTabs("}", 5);
            builder.AppendLineWithTabs("else", 5);
            builder.AppendLineWithTabs("{", 5);
            builder.AppendLineWithTabs("throw;", 6);
            builder.AppendLineWithTabs("}", 5);
            builder.AppendLineWithTabs("}", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("entity.MarkOld();", 3);
            builder.AppendLineWithTabs("return entity;", 3);
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        protected virtual string ToAnonymousMethod()
        {
            var builder = new StringBuilder();

            builder.AppendLineWithTabs("private DynamicParameters ToAnonymous(T entity, bool includeRowVersion)", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("dynamic anonymousEntity = entity.ToAnonymous();", 3);
            builder.AppendLineWithTabs("anonymousEntity.Id = entity.Id;", 3);
            builder.AppendLineWithTabs("var parameters = new DynamicParameters(anonymousEntity);", 3);
            builder.AppendLineWithTabs("if (includeRowVersion)", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("parameters.Add(\"RowVersion\", entity.RowVersion, DbType.Binary);", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("return parameters;", 3);
            builder.AppendLineWithTabs("}", 2);

            return builder.ToString();
        }

        protected virtual ClassFile BuildClassFile()
        {
            var classFile = FileManager.StartNewClassFile(GetGenericRepositoryName, Classlocation.Project, Classlocation.Folder);
            classFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", "Dapper", "System.Data", Interfacelocation.GetProjectLocation, "System.Linq", "System.Collections.Generic" });
            classFile.Namespaces.AddRange(Information.Repository.Namespaces.Select(x => x.Value).Concat(Information.GenericRepository.Namespaces.Select(x => x.Value)));
            classFile.Namespaces.Add("System.Globalization");
            classFile.OwnNamespace = Information.Repository.FileLocation.GetNamespace;
            classFile.NameExtension = "<T>";
            classFile.BaseClass = ": IGenericRepository<T> where T : EntityBase, new()";
            classFile.Constructor.Parameters.Add(new TypeWithName { Name = "connectionFactory", Type = "IConnectionFactory" });
            classFile.Constructor.CustomImplementation.Add("ConnectionFactory = connectionFactory;");
            classFile.Properties.Add("public IConnectionFactory ConnectionFactory { get; }");
            classFile.Methods.Add(SaveMethod());
            classFile.Methods.Add(InsertMethod());
            classFile.Methods.Add(UpdateMethod());
            classFile.Methods.Add(DeleteMethod());
            classFile.Methods.Add(ListSaveMethod());
            classFile.Methods.Add(GetByIdMethod());
            classFile.Methods.Add(ToAnonymousMethod());
            return classFile;
        }

        protected virtual InterfaceFile BuildInterfaceFile()
        {
            var interfaceFile = FileManager.StartNewInterfaceFile(GetInterfaceGenericRepositoryName, Interfacelocation.Project, Interfacelocation.Folder);
            interfaceFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", Interfacelocation.GetProjectLocation, "System.Collections.Generic" });
            interfaceFile.Namespaces.AddRange(Information.Repository.Namespaces.Select(x => x.Value).Concat(Information.GenericRepository.Namespaces.Select(x => x.Value)));
            interfaceFile.OwnNamespace = Interfacelocation.GetNamespace;
            interfaceFile.NameExtension = "<T> where T : EntityBase, new()";
            interfaceFile.Properties.Add("IConnectionFactory ConnectionFactory { get; }");
            interfaceFile.Methods.Add("T Save(T entity);");
            interfaceFile.Methods.Add("T GetById(long id); ");
            interfaceFile.Methods.Add("IEnumerable<T> Save(IEnumerable<T> entities);");
            return interfaceFile;
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            return new List<FileBuilder.OutputFile> { BuildClassFile(), BuildInterfaceFile() };
        }
    }
}
