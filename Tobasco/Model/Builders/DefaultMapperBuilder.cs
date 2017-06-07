using System;
using System.Collections.Generic;
using System.Text;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Model.Builders.Base;
using Tobasco.Model.Properties;

namespace Tobasco.Model.Builders
{
    public class DefaultMapperBuilder : MapperBuilderBase
    {
        public DefaultMapperBuilder(EntityHandler entity) : base(entity)
        {
            
        }

        private string MappingMethod(Mapper mapper)
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs($"public {EntityHandler.Entity.Name} MapToObject({EntityHandler.GetEntityLocationOnId(mapper.FromTo.To).FileLocation.GetProjectLocation}.{EntityHandler.Entity.Name} objectToMapFrom)", 0);
            builder.AppendLineWithTabs("{", 1);
            builder.AppendLineWithTabs($"var objectToMapTo = new {EntityHandler.Entity.Name}", 2);
            builder.AppendLineWithTabs("{", 2);
            foreach (var property in EntityHandler.GetClassBuilder(mapper.FromTo.To).GetNonChildCollectionProperties)
            {
                builder.AppendLineWithTabs($"{GetMappingProperty(property)},", 3);
            }
            builder.AppendLineWithTabs("};", 2);
            builder.Append(Environment.NewLine);
            foreach (var childcollectionProp in EntityHandler.GetClassBuilder(mapper.FromTo.To).GetChildCollectionProperties)
            {
                builder.AppendLineWithTabs($"foreach(var property in objectToMapFrom.{childcollectionProp.Property.Name})", 2);
                builder.AppendLineWithTabs("{", 2);
                builder.AppendLineWithTabs($"objectToMapTo.{GetMappingProperty(childcollectionProp)}", 3);
                builder.AppendLineWithTabs("}", 2);
                builder.Append(Environment.NewLine);
            }
            builder.AppendLineWithTabs("return objectToMapTo;", 2);
            builder.AppendLineWithTabs("}", 1);
            return builder.ToString();
        }

        private string GetMappingProperty(ClassProperty prop)
        {
            switch (prop.Property.DataType.Datatype)
            {
                case Datatype.Child:
                    return prop.Property.Name + " = _" + EntityHandler.GetMapperInterfaceParameter(prop.Property.DataType.Type) + ".MapToObject(objectToMapFrom." + prop.Property.Name + ")";
                case Datatype.ChildCollection:
                    return prop.Property.Name + ".Add(_" + EntityHandler.GetMapperInterfaceParameter(prop.Property.DataType.Type) + ".MapToObject(property));";
                default:
                    return prop.Property.Name + " = objectToMapFrom." + prop.Property.Name;
            }
        }

        public override IEnumerable<FileBuilder.OutputFile> Build(Mapper mapper)
        {
            var classFile = FileManager.StartNewClassFile(MapperName, mapper.MapperLocation.Project, mapper.MapperLocation.Folder);
            var fromEntityLocation = EntityHandler.GetEntityLocationOnId(mapper.FromTo.From).FileLocation.GetProjectLocation;
            classFile.BaseClass = $": {SelectMapperInterface}";
            classFile.OwnNamespace = mapper.MapperLocation.GetNamespace;
            classFile.Namespaces.Add(mapper.InterfaceLocation.GetProjectLocation);
            classFile.Namespaces.Add(fromEntityLocation);
            foreach (var property in EntityHandler.GetClassBuilder(mapper.FromTo.From).GetChildChildCollectionProperties)
            {
                classFile.Constructor.ParameterWithField.Add(new FieldWithParameter
                {
                    Parameter = new TypeWithName {Name = EntityHandler.GetMapperInterfaceParameter(property.Property.DataType.Type) , Type = EntityHandler.GetMapperInterface(property.Property.DataType.Type) } ,
                    Field = new TypeWithName { Name = EntityHandler.GetMapperInterfaceParameter(property.Property.DataType.Type), Type = EntityHandler.GetMapperInterface(property.Property.DataType.Type) }
                });
            }
            classFile.Methods.Add(MappingMethod(mapper));

            var interfacefile = FileManager.StartNewInterfaceFile(SelectMapperInterface, mapper.MapperLocation.Project, mapper.MapperLocation.Folder);
            var mapToLocation = EntityHandler.GetEntityLocationOnId(mapper.FromTo.To).FileLocation.GetProjectLocation;
            interfacefile.Namespaces.Add(fromEntityLocation);
            interfacefile.OwnNamespace = mapper.InterfaceLocation.GetNamespace;
            interfacefile.Methods.Add($"{EntityHandler.Entity.Name} MapToObject({mapToLocation}.{EntityHandler.Entity.Name} objectToMapFrom);");

            return new List<FileBuilder.OutputFile> { classFile, interfacefile };
        }
    }
}