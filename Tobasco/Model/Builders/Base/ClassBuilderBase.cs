using System.Collections.Generic;
using System.Linq;
using Tobasco.Enums;
using Tobasco.Factories;
using Tobasco.Model.Properties;

namespace Tobasco.Model.Builders.Base
{
	public abstract class ClassBuilderBase
	{
		protected readonly Entity Entity;
		protected readonly EntityLocation Location;
		private IEnumerable<ClassProperty> _getProperties;
		private IEnumerable<ClassProperty> _getChildChildCollectionProperties;
		private IEnumerable<ClassProperty> _getNonChildCollectionProperties;
		private readonly PropertyClassFactory _propertyFactory;

		private IEnumerable<ClassProperty> _getChildCollectionProperties;

		private IEnumerable<ClassProperty> _ghildNotCollectionProperties;

		protected ClassBuilderBase(Entity entity, EntityLocation location)
		{
			Entity = entity;
			Location = location;
			_propertyFactory = new PropertyClassFactory(location.ORMapper, location.GenerateRules);
		}

		public IEnumerable<ClassProperty> GetProperties
		{
			get
			{
				return _getProperties ?? (_getProperties = Entity.Properties.Select(x => _propertyFactory.GetProperty(x)));
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
				if (_getChildCollectionProperties == null)
				{
					_getChildCollectionProperties = GetProperties.Where(x =>
						x.Property.DataType.Datatype == Datatype.ChildCollection);
				}

				return _getChildCollectionProperties;
			}
		}

		public IEnumerable<ClassProperty> GetChildNotCollectionProperties
		{
			get
			{
				if (_ghildNotCollectionProperties == null)
				{
					_ghildNotCollectionProperties = GetProperties.Where(x =>
						x.Property.DataType.Datatype == Datatype.Child || x.Property.DataType.Datatype == Datatype.ReadonlyChild);
				}

				return _ghildNotCollectionProperties;
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
