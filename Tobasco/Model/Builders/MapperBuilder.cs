using System;
using System.Collections.Generic;
using System.Text;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.FileBuilder;
using Tobasco.Model.Properties;

namespace Tobasco.Model.Builders
{
    public class MapperBuilder
    {
        private EntityHandler _entity;

        public MapperBuilder(EntityHandler entity)
        {
            _entity = entity;
        }

        public string MapperName => $"{_entity.Entity.Name}Mapper";

        public string SelectMapperParameter => $"{char.ToLowerInvariant(_entity.Entity.Name[0])}{_entity.Entity.Name.Substring(1)}Mapper";

        public string SelectMapperInterface => $"I{_entity.Entity.Name}Mapper";

        private string MappingMethod(Mapper mapper)
        {
            var builder = new StringBuilder();
            builder.AppendLineWithTabs($"public {_entity.Entity.Name} MapToObject({_entity.GetEntityLocationOnId(mapper.FromTo.To).GetProjectLocation}.{_entity.Entity.Name} objectToMapFrom)", 0);
            builder.AppendLineWithTabs("{", 1);
            builder.AppendLineWithTabs($"var objectToMapTo = new {_entity.Entity.Name}", 2);
            builder.AppendLineWithTabs("{", 2);
            foreach (var property in _entity.GetClassBuilder(mapper.FromTo.To).GetNonChildCollectionProperties)
            {
                builder.AppendLineWithTabs($"{GetMappingProperty(property)},", 3);
            }
            builder.AppendLineWithTabs("};", 2);
            builder.Append(Environment.NewLine);
            foreach (var childcollectionProp in _entity.GetClassBuilder(mapper.FromTo.To).GetChildCollectionProperties)
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
                    return prop.Property.Name + " = _" + _entity.GetMapperInterfaceParameter(prop.Property.DataType.Type) + ".MapToObject(objectToMapFrom." + prop.Property.Name + ")";
                case Datatype.ChildCollection:
                    return prop.Property.Name + ".Add(_" + _entity.GetMapperInterfaceParameter(prop.Property.DataType.Type) + ".MapToObject(property));";
                default:
                    return prop.Property.Name + " = objectToMapFrom." + prop.Property.Name;
            }
        }

        public IEnumerable<FileBuilder.OutputFile> Build(Mapper mapper)
        {
            var classFile = (ClassFile)FileManager.StartNewFile(MapperName, mapper.MapperLocation.Project, mapper.MapperLocation.Folder, FileType.Class);
            var fromEntityLocation = _entity.GetEntityLocationOnId(mapper.FromTo.From).GetProjectLocation;
            classFile.BaseClass = $": {SelectMapperInterface}";
            classFile.OwnNamespace = mapper.MapperLocation.GetNamespace;
            classFile.Namespaces.Add(mapper.InterfaceLocation.GetProjectLocation);
            classFile.Namespaces.Add(fromEntityLocation);
            foreach (var property in _entity.GetClassBuilder(mapper.FromTo.From).GetChildChildCollectionProperties)
            {
                classFile.Constructor.ParameterWithField.Add(new FieldWithParameter
                {
                    Name = _entity.GetMapperInterfaceParameter(property.Property.DataType.Type),
                    Type = _entity.GetMapperInterface(property.Property.DataType.Type),
                    Field = $"_{_entity.GetMapperInterfaceParameter(property.Property.DataType.Type)}"
                });
            }
            classFile.Methods.Add(MappingMethod(mapper));

            var interfacefile = (InterfaceFile)FileManager.StartNewFile(SelectMapperInterface, mapper.MapperLocation.Project, mapper.MapperLocation.Folder, FileType.Interface);
            var mapToLocation = _entity.GetEntityLocationOnId(mapper.FromTo.To).GetProjectLocation;
            interfacefile.Namespaces.Add(fromEntityLocation);
            interfacefile.OwnNamespace = mapper.InterfaceLocation.GetNamespace;
            interfacefile.Methods.Add($"{_entity.Entity.Name} MapToObject({mapToLocation}.{_entity.Entity.Name} objectToMapFrom);");

            return new List<FileBuilder.OutputFile> { classFile, interfacefile };
        }
    }
}