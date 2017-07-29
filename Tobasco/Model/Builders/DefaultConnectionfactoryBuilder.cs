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
            classFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", interfacelocation.GetProjectLocation });
            classFile.OwnNamespace = classlocation.GetNamespace;
            classFile.BaseClass = $": {GetConnectionFactoryInterfaceName}";
            classFile.Constructor.Fields.Add(new Field {Type = "string", Name = "_connectionstring", Modifier = "private readonly" });
            classFile.Constructor.Parameters.Add(new TypeWithName { Type = "string", Name = "databasenaam" });
            classFile.Constructor.CustomImplementation.Add("_connectionstring = ConfigurationManager.ConnectionStrings[databasenaam].ConnectionString;");
            classFile.Methods.Add(GetConnectionMethod());

            var interfaceFile = FileManager.StartNewInterfaceFile(GetConnectionFactoryInterfaceName, interfacelocation.Project, interfacelocation.Folder);
            interfaceFile.Namespaces.Add("System.Data.SqlClient");
            interfaceFile.OwnNamespace = Information.Repository.InterfaceLocation.GetNamespace;
            interfaceFile.Methods.Add("SqlConnection GetConnection();");
            return new List<FileBuilder.OutputFile> { classFile, interfaceFile };
        }

        private string GetConnectionMethod()
        {
            return Resources.ConnectionFactoryGetConnection;
        }
    }
}
