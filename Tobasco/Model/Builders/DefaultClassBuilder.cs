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
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
    public class DefaultClassBuilder : ClassBuilderBase
    {

        public DefaultClassBuilder(Entity entity, EntityLocation location) : base(entity, location) 
        {
        }


        private string GenerateMethods()
        {
            var template = new Template();
            var orm = Location.ORMapper?.Type ?? OrmType.Onbekend;

            switch (orm)
            {
                case OrmType.Dapper:
                    template.SetTemplate(Resources.ClassDapperMethod);
                    template.Fill(GetParameters());
                    return template.GetText;
                default:
                    return string.Empty;
            }
        }

        protected virtual TemplateParameter GetParameters()
        {
            var list = new List<string>();
            foreach (var property in GetProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection))
            {
                if (property.Property.DataType.Datatype == Datatype.Child || property.Property.DataType.Datatype == Datatype.ReadonlyChild)
                {
                    if (property.Property.Required)
                    {
                        list.Add($"anymonous.{property.Property.Name}Id = {property.Property.Name}.Id;");
                    }
                    else
                    {
                        list.Add($"anymonous.{property.Property.Name}Id = {property.Property.Name}?.Id;");
                    }
                }
                else
                {
                    list.Add($"anymonous.{property.Property.Name} = {property.Property.Name};");
                }
            }

            var parameters = new TemplateParameter();
            parameters.Add(Resources.AnymonousPropertySet, list);
            return parameters;            
        }

        public override IEnumerable<FileBuilder.OutputFile> Build()
        {
            OutputPaneManager.WriteToOutputPane($"Build {Entity.Name} for {Location.FileLocation.GetProjectLocation}");
            var file = FileManager.StartNewClassFile(Entity.Name, Location.FileLocation.Project, Location.FileLocation.Folder);
            file.ClassAttributes.Add("[Serializable]");
            file.IsAbstract = Entity.IsAbstract;
            file.BaseClass = Location.GetBaseClass;
            file.Namespaces.AddRange(Location.Namespaces.Select(x => x.Value).Concat(MainInfoManager.GetBasicNamespaces));
            file.Namespaces.Add(MainInfoManager.GetEnumNamespace);        
            file.Properties.AddRange(GetProperties.Select(x => x.GetProperty));
            file.Methods.Add(GenerateMethods());
            file.OwnNamespace = Location.FileLocation.GetNamespace;
            return new List<FileBuilder.OutputFile> { file };
        }
    }
}
