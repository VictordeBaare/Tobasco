using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.Factories;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.Properties;

namespace Tobasco.Model.Builders
{
    public class DefaultClassBuilder : ClassBuilderBase
    {

        public DefaultClassBuilder(Entity entity, EntityLocation location, EntityInformation information) : base(entity, location, information) 
        {
        }


        private string GenerateMethods()
        {
            var builder = new StringBuilder();
            var orm = Location.ORMapper?.Type ?? OrmType.Onbekend;

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
                    foreach (var property in GetProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection))
                    {
                        if (property.Property.DataType.Datatype == Datatype.Child || property.Property.DataType.Datatype == Datatype.ReadonlyChild)
                        {
                            if (property.Property.Required)
                            {
                                builder.AppendWithTabs($"anymonous.{property.Property.Name}Id = {property.Property.Name}.Id;", 3);
                            }
                            else
                            {
                                builder.AppendWithTabs($"anymonous.{property.Property.Name}Id = {property.Property.Name}?.Id;", 3);
                            }
                        }
                        else
                        {
                            builder.AppendWithTabs($"anymonous.{property.Property.Name} = {property.Property.Name};", 3);
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

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            OutputPaneManager.WriteToOutputPane($"Build {Entity.Name} for {Location.FileLocation.GetProjectLocation}");
            var file = FileManager.StartNewClassFile(Entity.Name, Location.FileLocation.Project, Location.FileLocation.Folder);
            file.ClassAttributes.Add("[Serializable]");
            file.IsAbstract = Entity.IsAbstract;
            file.BaseClass = Location.GetBaseClass;
            file.Namespaces.AddRange(Location.Namespaces.Select(x => x.Value).Concat(Information.Namespaces.Select(x => x.Value)));
            file.Properties.AddRange(GetProperties.Select(x => x.GetProperty));
            file.Methods.Add(GenerateMethods());
            file.OwnNamespace = Location.FileLocation.GetNamespace;
            return new List<FileBuilder.OutputFile> { file };
        }
    }
}
