﻿using System.Collections.Generic;
using System.Linq;
using Tobasco.Factories;
using Tobasco.Model.Properties;
using Tobasco.Enums;

namespace Tobasco.Model.Builders.Base
{
    public abstract class ClassBuilderBase
    {
        protected readonly Entity Entity;
        protected readonly EntityLocation Location;
        protected readonly EntityInformation Information;
        private IEnumerable<ClassProperty> _getProperties;
        private IEnumerable<ClassProperty> _getChildChildCollectionProperties;
        private IEnumerable<ClassProperty> _getNonChildCollectionProperties;
        private readonly PropertyClassFactory _propertyFactory;

        protected ClassBuilderBase(Entity entity, EntityLocation location, EntityInformation information)
        {
            Entity = entity;
            Location = location;
            Information = information;
            _propertyFactory = new PropertyClassFactory(location.ORMapper, location.GenerateRules);
        }

        public IEnumerable<ClassProperty> GetProperties
        {
            get
            {
                if (_getProperties == null)
                {
                     List<ClassProperty>  properties = Entity.Properties.Select(x => _propertyFactory.GetProperty(x)).ToList();

                    if (Entity.GenerateReadonlyGuid)
                    {
                        properties.Add(_propertyFactory.GetProperty(new Property
                        {
                            DataType = new DataType {Datatype = Datatype.ReadOnlyGuid},
                            Name = "Uid",
                            Required = true
                        }));
                    }

                    _getProperties = properties;
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
                if (_getNonChildCollectionProperties == null)
                {
                    _getNonChildCollectionProperties = GetProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection);
                }
                return _getNonChildCollectionProperties;
            }
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}
