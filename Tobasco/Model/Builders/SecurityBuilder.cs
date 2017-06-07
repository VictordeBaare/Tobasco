using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Factories;
using Tobasco.FileBuilder;
using Tobasco.Extensions;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.Builders.Security;

namespace Tobasco.Model.Builders
{
    public class SecurityBuilder : SecurityBuilderBase
    {
        public SecurityBuilder(EntityHandler entityHandler, EntityInformation entityInformation) : base(entityHandler, entityInformation)
        {
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            var securityElement = GetSecurityElement();
            var outputFiles = new List<FileBuilder.OutputFile>();
            if (securityElement != null && securityElement.Generate)
            {
                outputFiles.AddRange(GenerateDac(securityElement.Dac));
                OutputPaneManager.WriteToOutputPane($"{outputFiles.Count} security files generated.");
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("No security files will be generated.");
            }
            return outputFiles;
        }

        private List<FileBuilder.OutputFile> GenerateDac(Dac dac)
        {
            var dacFiles = new List<FileBuilder.OutputFile>();
            if (dac != null && dac.Generate)
            {
                dacFiles.Add(GetClassFile(dac));
                dacFiles.AddRange(GetSqlFiles(dac));
                dacFiles.AddRange(GetSecurityRepositoryBuilder(dac).Build());
            }
            return dacFiles;
        }

        private IEnumerable<FileBuilder.OutputFile> GetSqlFiles(Dac dac)
        {
            var type = BuilderManager.Get("", DefaultBuilderConstants.SecurityDatabaseBuilder);
            var builder = BuilderManager.InitializeBuilder<DatabaseBuilderBase>(type, new object[] {Entity, dac });
            return builder.Build();
        }

        public SecurityRepositoryBase GetSecurityRepositoryBuilder(Dac dac)
        {
            var type = BuilderManager.Get(dac.Repository.Overridekey, DefaultBuilderConstants.SecurityRepositoryBuilder);
            return BuilderManager.InitializeBuilder<SecurityRepositoryBase>(type, new object[] { EntityHandler, dac });
        }

        private ClassFile GetClassFile(Dac dac)
        {
            var propertyFactory = new PropertyClassFactory(dac.ORMapper, dac.Generate);
            var classFile = FileManager.StartNewClassFile(Entity.Name + "Dac", dac.FileLocation.Project,
                dac.FileLocation.Folder);
            classFile.Namespaces.AddRange(EntityInformation.Namespaces.Select(x => x.Value));
            classFile.Namespaces.AddRange(new[] {"System.ComponentModel.DataAnnotations", "System.Dynamic"});
            classFile.BaseClass = dac.GetBaseClass;
            classFile.OwnNamespace = dac.FileLocation.GetNamespace;
            classFile.Properties.AddRange(dac.Properties.Select(x => propertyFactory.GetProperty(x).GetProperty));
            classFile.Methods.Add(GenerateMethods(dac));
            return classFile;
        }

        private string GenerateMethods(Dac dac)
        {
            var builder = new StringBuilder();
            var orm = dac.ORMapper?.Type ?? OrmType.Onbekend;

            switch (orm)
            {
                case OrmType.Dapper:
                    builder.Append("public override ExpandoObject ToAnonymous()");
                    builder.Append(Environment.NewLine);
                    builder.AppendWithTabs("{", 2);
                    builder.Append(Environment.NewLine);
                    builder.AppendWithTabs("dynamic anymonous = base.ToAnonymous();", 3);
                    builder.Append(Environment.NewLine);
                    builder.Append(Environment.NewLine);
                    foreach (var property in dac.Properties.Where(x => x.DataType.Datatype != Datatype.ChildCollection))
                    {
                        if (property.DataType.Datatype == Datatype.Child)
                        {
                            if (property.Required)
                            {
                                builder.AppendWithTabs($"anymonous.{property.Name}Id = {property.Name}.Id;", 3);
                            }
                            else
                            {
                                builder.AppendWithTabs($"anymonous.{property.Name}Id = {property.Name}?.Id;", 3);
                            }
                        }
                        else
                        {
                            builder.AppendWithTabs($"anymonous.{property.Name} = {property.Name};", 3);
                        }
                        builder.Append(Environment.NewLine);
                    }
                    builder.AppendWithTabs("return anymonous;", 3);
                    builder.Append(Environment.NewLine);
                    builder.AppendWithTabs("}", 2);
                    return builder.ToString();
                default:
                    return builder.ToString();
            }
        }
    }
}