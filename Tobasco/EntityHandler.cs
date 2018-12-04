using System;
using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Manager;
using Tobasco.Model;
using Tobasco.Model.Builders.Base;

namespace Tobasco
{
	public class EntityHandler
	{
		private readonly Entity _entity;
        private readonly bool _isChanged;
        private readonly Dictionary<string, ClassBuilderBase> _classBuilderDictionary;

		public EntityHandler(Entity entity, bool isChanged)
		{
			_entity = entity;
            _isChanged = isChanged;
            _classBuilderDictionary = new Dictionary<string, ClassBuilderBase>();
		}

		public string GetMapperInterface(string naam)
		{
			var handler = EntityManager.GetEntityOnName(naam);
			return handler.GetMapperBuilder.SelectMapperInterface;
		}

		public string GetMapperInterfaceParameter(string naam)
		{
			var handler = EntityManager.GetEntityOnName(naam);
			return handler.GetMapperBuilder.SelectMapperParameter;
		}

		public string GetRepositoryInterface(string naam)
		{
			var handler = EntityManager.GetEntityOnName(naam);
			return handler.GetRepositoryBuilder.GetRepositoryInterfaceName;
		}

		public string GetRepositoryOnName(string naam)
		{
			var handler = EntityManager.GetEntityOnName(naam);
			return handler.GetRepositoryBuilder.GetRepositoryName;
		}

		public Property GetChildReferencePropertyIfExists(string type, string parent)
		{
			var handler = EntityManager.GetEntityOnName(type);
			if (handler == null)
			{
				throw new ArgumentNullException($"No handler was found with the type: {type}");
			}
			var property = handler.Entity.Properties.FirstOrDefault(x => x.DataType.Type == parent && x.DataType.Datatype == Datatype.Reference);
			return property;
		}

		public string GetProjectLocation(string naam, string id)
		{
			var handler = EntityManager.GetEntityOnName(naam);
			return handler.GetEntityLocationOnId(id).FileLocation.GetProjectLocation;
		}

		public string SelectChildMapperInterfaces(string id)
		{
			List<string> namen = new List<string>();
			foreach (var prop in GetClassBuilder(id).GetChildChildCollectionProperties)
			{
				namen.Add(GetMapperInterface(prop.Property.DataType.Type) + " " + GetMapperInterfaceParameter(prop.Property.DataType.Type));
			}

			return string.Join(", ", namen);
		}

		public IEnumerable<string> SelectChildRepositoryInterfaces(IEnumerable<Property> properties)
		{
			List<string> childRepositories = new List<string>();
			foreach (var childProp in properties)
			{
				childRepositories.Add(GetRepositoryInterface(childProp.DataType.Type));
			}
			return childRepositories;
		}

		public IEnumerable<string> GetChildProjectLocationNames()
		{
			List<string> childRepositories = new List<string>();
			foreach (var childProp in _entity.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection || x.DataType.Datatype == Datatype.Child))
			{
				childRepositories.Add(GetRepositoryInterface(childProp.DataType.Type));
			}
			return childRepositories;

		}

		public IEnumerable<EntityLocation> GetEntityLocations
		{
			get
			{
				if (_entity.EntityLocations != null && _entity.EntityLocations.Any())
				{
					return _entity.EntityLocations;
				}
				return MainInfoManager.EntityInformation.EntityLocations;
			}
		}

		public Repository GetRepository
		{
			get
			{
				if (_entity.Repository != null)
				{
					return _entity.Repository;
				}
				return MainInfoManager.EntityInformation.Repository;
			}
		}

		public Database GetDatabase
		{
			get
			{
				if (_entity.Database != null)
				{
					return _entity.Database;
				}
				return MainInfoManager.EntityInformation.Database;
			}
		}

		public Mappers GetMappers
		{
			get
			{
				if (_entity.Mappers != null)
				{
					return _entity.Mappers;
				}
				return MainInfoManager.EntityInformation.Mappers;
			}
		}

		public EntityLocation GetEntityLocationOnId(string id)
		{
			var location = GetEntityLocations.FirstOrDefault(x => x.Id == id);
			if (location != null)
			{
				return location;
			}
			throw new ArgumentException();
		}

		public Entity Entity => _entity;

        public bool IsChanged => _isChanged;

		public ClassBuilderBase GetClassBuilder(EntityLocation entitylocation)
		{
			if (!string.IsNullOrEmpty(entitylocation.Id) && _classBuilderDictionary.ContainsKey(entitylocation.Id))
			{
				return _classBuilderDictionary[entitylocation.Id];
			}
			var type = BuilderManager.Get(entitylocation.Overridekey, DefaultBuilderConstants.ClassBuilder);
			var classBuilder = BuilderManager.InitializeBuilder<ClassBuilderBase>(type, new object[] { _entity, entitylocation });

			var id = entitylocation.Id ?? "LocationId" + _classBuilderDictionary.Count + 1;
			_classBuilderDictionary.Add(id, classBuilder);
			return classBuilder;
		}

		private string GetDatabaseOverrideKey()
		{
			if (!string.IsNullOrEmpty(GetDatabase?.Overridekey))
			{
				return GetDatabase.Overridekey;
			}
			return MainInfoManager.EntityInformation.Database?.Overridekey;
		}

		private string GetMappersOverrideKey()
		{
			if (!string.IsNullOrEmpty(GetMappers?.Overridekey))
			{
				return GetMappers.Overridekey;
			}
			return MainInfoManager.EntityInformation.Mappers?.Overridekey;
		}

		private string GetRepositoryOverrideKey()
		{
			if (!string.IsNullOrEmpty(GetRepository?.Overridekey))
			{
				return GetRepository.Overridekey;
			}
			return MainInfoManager.EntityInformation.Repository?.Overridekey;
		}

		public ClassBuilderBase GetClassBuilder(string id)
		{
			if (_classBuilderDictionary.ContainsKey(id))
			{
				return _classBuilderDictionary[id];
			}
			throw new ArgumentException();
		}

		public DatabaseBuilderBase GetDatabaseBuilder
		{
			get
			{
				var type = BuilderManager.Get(GetDatabaseOverrideKey(), DefaultBuilderConstants.DatabaseBuilder);
				return BuilderManager.InitializeBuilder<DatabaseBuilderBase>(type, new object[] { Entity, GetDatabase });
			}
		}

		public MapperBuilderBase GetMapperBuilder
		{
			get
			{
				var type = BuilderManager.Get(GetMappersOverrideKey(), DefaultBuilderConstants.MapperBuilder);
				return BuilderManager.InitializeBuilder<MapperBuilderBase>(type, new object[] { this });
			}
		}

		public RepositoryBuilderBase GetRepositoryBuilder
		{
			get
			{
				var type = BuilderManager.Get(GetRepositoryOverrideKey(), DefaultBuilderConstants.RepositoryBuilder);
				return BuilderManager.InitializeBuilder<RepositoryBuilderBase>(type, new object[] { this, GetRepository });
			}
		}
	}
}
