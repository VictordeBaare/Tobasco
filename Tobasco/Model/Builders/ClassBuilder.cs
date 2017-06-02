using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Enums;
using Tobasco.Extensions;
using Tobasco.Factories;
using Tobasco.FileBuilder;
using Tobasco.Model.Properties;

namespace Tobasco.Model.Builders
{
    public class ClassBuilder
    {
        private readonly PropertyClassFactory _propertyFactory;
        private IEnumerable<ClassProperty> _getProperties;
        private readonly Entity _entity;
        private readonly EntityLocation _location;
        private readonly EntityInformation _information;
        private IEnumerable<ClassProperty> _getChildChildCollectionProperties;
        private IEnumerable<ClassProperty> _getNonChildCollectionProperties;

        public ClassBuilder(Entity entity, EntityLocation location, EntityInformation information)
        {
            _propertyFactory = new PropertyClassFactory(location);
            _entity = entity;
            _location = location;
            _information = information;
        }

        public IEnumerable<ClassProperty> GetProperties
        {
            get
            {
                if (_getProperties == null)
                {
                    _getProperties = _entity.Properties.Select(x => _propertyFactory.GetProperty(x));
                }

                return _getProperties;
            }
        }

        public IEnumerable<ClassProperty> GetChildChildCollectionProperties
        {
            get
            {
                if (_getChildChildCollectionProperties == null)
                {
                    _getChildChildCollectionProperties = GetProperties.Where(x => 
                        x.Property.DataType.Datatype == Datatype.Child || 
                        x.Property.DataType.Datatype == Datatype.ChildCollection);
                }

                return _getChildChildCollectionProperties;
            }
        }

        public IEnumerable<ClassProperty> GetChildCollectionProperties
        {
            get
            {
                if (_getChildChildCollectionProperties == null)
                {
                    _getChildChildCollectionProperties = GetProperties.Where(x =>
                        x.Property.DataType.Datatype == Datatype.ChildCollection);
                }

                return _getChildChildCollectionProperties;
            }
        }

        public IEnumerable<ClassProperty> GetNonChildCollectionProperties
        {
            get
            {
                if(_getNonChildCollectionProperties == null)
                {
                    _getNonChildCollectionProperties = GetProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection);
                }
                return _getNonChildCollectionProperties;
            }
        }

        private string GenerateMethods()
        {
            var builder = new StringBuilder();
            var orm = _location.ORMapper?.Type ?? OrmType.Onbekend;

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
                        if (property.Property.DataType.Datatype == Datatype.Child)
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

        public FileBuilder.OutputFile Build(DynamicTextTransformation2 textTransformation)
        {
            textTransformation.WriteLine($"// Build {_entity.Name} for {_location.GetProjectLocation}");
            var file = FileManager.StartNewClassFile(_entity.Name, _location.Project, _location.Folder);
            file.IsAbstract = _entity.IsAbstract;
            file.BaseClass = _location.GetBaseClass;
            file.Namespaces.AddRange(_location.Namespaces.Select(x => x.Value).Concat(_information.Namespaces.Select(x => x.Value)));
            file.Properties.AddRange(GetProperties.Select(x => x.GetProperty));
            file.Methods.Add(GenerateMethods());
            file.OwnNamespace = _location.GetNamespace;
            return file;
        }
    }
}
