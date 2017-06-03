using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _propertyFactory = new PropertyClassFactory(location);
        }

        public IEnumerable<ClassProperty> GetProperties
        {
            get
            {
                if (_getProperties == null)
                {
                    _getProperties = Entity.Properties.Select(x => _propertyFactory.GetProperty(x));
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

        public abstract FileBuilder.OutputFile Build();
    }
}
