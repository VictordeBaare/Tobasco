using System.Collections.Generic;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;
using Tobasco.Properties;

namespace Tobasco.Model.Builders
{
    public class DefaultConnectionfactoryBuilder : ConnectionfactoryBuilderBase
    {
        public DefaultConnectionfactoryBuilder(EntityInformation information) : base(information) { }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            var classFile = FileManager.StartNewClassFile(GetConnectionFactoryName, classlocation.Project, classlocation.Folder);
            classFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", "System.Data", interfacelocation.GetProjectLocation });
            classFile.OwnNamespace = classlocation.GetNamespace;
            classFile.BaseClass = $": {GetConnectionFactoryInterfaceName}";
            classFile.Constructor.Fields.Add(new Field("private readonly", "_connection", "SqlConnection"));
            classFile.Constructor.Fields.Add(new Field("private readonly", "_lock = new object()", "object"));
            classFile.Constructor.Fields.Add(new Field("private", "_disposed", "bool"));
            classFile.Constructor.Parameters.Add(new TypeWithName("databasenaam", "string"));
            classFile.Constructor.CustomImplementation.Add("_connection = new SqlConnection(ConfigurationManager.ConnectionStrings[databasenaam].ConnectionString);");
            classFile.Methods.Add(GetConnectionMethod());
            classFile.Methods.Add(Resources.PublicDispose);
            classFile.Methods.Add(Resources.PrivateDispose);

            var interfaceFile = FileManager.StartNewInterfaceFile(GetConnectionFactoryInterfaceName, interfacelocation.Project, interfacelocation.Folder);
            interfaceFile.Namespaces.Add("System.Data.SqlClient");
            interfaceFile.OwnNamespace = Information.Repository.InterfaceLocation.GetNamespace;
            interfaceFile.Methods.Add("SqlConnection GetConnection();");
            return new List<FileBuilder.OutputFile> { classFile, interfaceFile };
        }

        protected virtual string GetConnectionMethod()
        {
            return Resources.ConnectionFactoryGetConnection;
        }
    }
}
