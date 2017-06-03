using System;
using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Manager;
using Tobasco.Model;
using Tobasco.Model.Builders;
using Tobasco.Model.Builders.Base;

namespace Tobasco
{
    public class EntityHandler
    {
        private readonly Entity _entity;
        private readonly EntityInformation _mainInformation;
        private DefaultDatabaseBuilder _database;
        private readonly Dictionary<string, ClassBuilderBase> _classBuilderDictionary;
        private readonly Func<string, EntityHandler> _getHandlerOnName;

        public EntityHandler(Entity entity, EntityInformation mainInformation, Func<string, EntityHandler> getHandlerOnName)
        {
            _entity = entity;
            _mainInformation = mainInformation;
            _classBuilderDictionary = new Dictionary<string, ClassBuilderBase>();
            _getHandlerOnName = getHandlerOnName;
        }
        
        public string GetMapperInterface(string naam)
        {
            var handler = _getHandlerOnName(naam);
            return handler.GetMapperBuilder.SelectMapperInterface;
        }

        public string GetMapperInterfaceParameter(string naam)
        {
            var handler = _getHandlerOnName(naam);
            return handler.GetMapperBuilder.SelectMapperParameter;
        }

        public string GetRepositoryInterface(string naam)
        {
            var handler = _getHandlerOnName(naam);
            return handler.GetRepositoryBuilder.GetRepositoryInterfaceName;
        }

        public Property GetChildReferenceProperty(string type, string parent)
        {
            var handler = _getHandlerOnName(type);
            return handler.Entity.Properties.First(x => x.DataType.Type == parent && x.DataType.Datatype == Datatype.Reference);
        }

        public string GetProjectLocation(string naam, string id)
        {
            var handler = _getHandlerOnName(naam);
            return handler.GetEntityLocationOnId(id).GetProjectLocation;
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

        public IEnumerable<string> SelectChildRepositoryInterfaces()
        {
            List<string> childRepositories = new List<string>();
            foreach (var childProp in _entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.ChildCollection || x.DataType.Datatype == Enums.Datatype.Child))
            {
                childRepositories.Add(GetRepositoryInterface(childProp.DataType.Type));
            }
            return childRepositories;
        }

        public IEnumerable<string> GetChildProjectLocationNames()
        {
            List<string> childRepositories = new List<string>();
            foreach (var childProp in _entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.ChildCollection || x.DataType.Datatype == Enums.Datatype.Child))
            {
                childRepositories.Add(GetRepositoryInterface(childProp.DataType.Type));
            }
            return childRepositories;

        }

        public IEnumerable<EntityLocation> GetEntityLocations
        {
            get
            {
                if(_entity.EntityLocations != null && _entity.EntityLocations.Any())
                {
                    return _entity.EntityLocations;
                }
                return _mainInformation.EntityLocations;
            }
        }

        public Repository GetRepository
        {
            get
            {
                if(_entity.Repository != null)
                {
                    return _entity.Repository;
                }
                return _mainInformation.Repository;
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
                return _mainInformation.Database;
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
                return _mainInformation.Mappers;
            }
        }

        public EntityLocation GetEntityLocationOnId(string id)
        {
            var location = GetEntityLocations.FirstOrDefault(x => x.Id == id);
            if(location != null)
            {
                return location;
            }
            throw new ArgumentException();
        }

        public Entity Entity => _entity;


        public ClassBuilderBase GetClassBuilder(EntityLocation entitylocation)
        {
            if (!string.IsNullOrEmpty(entitylocation.Id) &&_classBuilderDictionary.ContainsKey(entitylocation.Id))
            {
                return _classBuilderDictionary[entitylocation.Id];
            }
            var type = BuilderManager.Get(entitylocation.Id, DefaultBuilderConstants.ClassBuilder);
            var classBuilder = BuilderManager.InitializeBuilder<ClassBuilderBase>(type, new object[] { _entity, entitylocation, _mainInformation });

            var id = entitylocation.Id ?? "LocationId" + _classBuilderDictionary.Count + 1;
            _classBuilderDictionary.Add(id, classBuilder);
            return classBuilder;
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
                var type = BuilderManager.Get(_mainInformation.Database.Id, DefaultBuilderConstants.DatabaseBuilder);
                return BuilderManager.InitializeBuilder<DatabaseBuilderBase>(type, new object[] { this });
            }
        }

        public MapperBuilderBase GetMapperBuilder
        {
            get
            {
                var type = BuilderManager.Get(_mainInformation.Mappers.Id, DefaultBuilderConstants.MapperBuilder);
                return BuilderManager.InitializeBuilder<MapperBuilderBase>(type, new object[] { this });
            }
        }

        public RepositoryBuilderBase GetRepositoryBuilder
        {
            get
            {
                var type = BuilderManager.Get(_mainInformation.Repository.Id, DefaultBuilderConstants.RepositoryBuilder);
                return BuilderManager.InitializeBuilder<RepositoryBuilderBase>(type, new object[] {this, _mainInformation});
            }
        }
    }
}
