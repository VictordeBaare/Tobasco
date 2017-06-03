using System;
using System.Collections.Generic;
using System.Text;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;

namespace Tobasco.Model.Builders
{
    public class DefaultConnectionfactoryBuilder : ConnectionfactoryBuilderBase
    {
        public DefaultConnectionfactoryBuilder(EntityInformation information) : base(information) { }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            var classFile = FileManager.StartNewClassFile(GetConnectionFactoryName, classlocation.Project, classlocation.Folder);
            classFile.Namespaces.AddRange(new[] { "System.Configuration", "System.Data.SqlClient", interfacelocation.GetProjectLocation });
            classFile.OwnNamespace = Information.Repository.GetNamespace;
            classFile.BaseClass = $": {GetConnectionFactoryInterfaceName}";
            classFile.Fields.Add("private readonly string _connectionstring");
            classFile.Constructor.Parameters.Add(new Parameter { Type = "string", Name = "databasenaam" });
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
            var builder = new StringBuilder();
            builder.AppendLineWithTabs("public SqlConnection GetConnection()", 0);
            builder.AppendLineWithTabs("{", 2);
            builder.AppendLineWithTabs("SqlConnection connection = null;", 3);
            builder.AppendLineWithTabs("SqlConnection tempConnection = null;", 3);
            builder.AppendLineWithTabs("try", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("tempConnection = new SqlConnection(_connectionstring);", 4);
            builder.AppendLineWithTabs("tempConnection.Open();", 4);
            builder.AppendLineWithTabs("connection = tempConnection;", 4);
            builder.AppendLineWithTabs("tempConnection = null;", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.AppendLineWithTabs("finally", 3);
            builder.AppendLineWithTabs("{", 3);
            builder.AppendLineWithTabs("tempConnection?.Dispose();", 4);
            builder.AppendLineWithTabs("}", 3);
            builder.Append(Environment.NewLine);
            builder.AppendLineWithTabs("return connection;", 3);
            builder.AppendLineWithTabs("}", 2);
            return builder.ToString();
        }
    }
}
