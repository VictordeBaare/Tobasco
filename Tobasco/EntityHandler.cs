using System;
using System.Collections.Generic;
using System.Linq;
using Tobasco.Enums;
using Tobasco.Model;
using Tobasco.Model.Builders;

namespace Tobasco
{
    public class EntityHandler
    {
        private Entity entity;
        private EntityInformation mainInformation;
        private DatabaseBuilder _database;
        private Dictionary<string, ClassBuilder> _classBuilderDictionary;
        private Func<string, EntityHandler> _getHandlerOnName;

        public EntityHandler(Entity entity, EntityInformation mainInformation, Func<string, EntityHandler> getHandlerOnName)
        {
            this.entity = entity;
            this.mainInformation = mainInformation;
            _classBuilderDictionary = new Dictionary<string, ClassBuilder>();
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
            foreach (var childProp in entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.ChildCollection || x.DataType.Datatype == Enums.Datatype.Child))
            {
                childRepositories.Add(GetRepositoryInterface(childProp.DataType.Type));
            }
            return childRepositories;
        }

        public IEnumerable<string> GetChildProjectLocationNames()
        {
            List<string> childRepositories = new List<string>();
            foreach (var childProp in entity.Properties.Where(x => x.DataType.Datatype == Enums.Datatype.ChildCollection || x.DataType.Datatype == Enums.Datatype.Child))
            {
                childRepositories.Add(GetRepositoryInterface(childProp.DataType.Type));
            }
            return childRepositories;

        }

        public IEnumerable<EntityLocation> GetEntityLocations
        {
            get
            {
                if(entity.EntityLocations != null && entity.EntityLocations.Any())
                {
                    return entity.EntityLocations;
                }
                return mainInformation.EntityLocations;
            }
        }

        public Repository GetRepository
        {
            get
            {
                if(entity.Repository != null)
                {
                    return entity.Repository;
                }
                return mainInformation.Repository;
            }
        }

        public Database GetDatabase
        {
            get
            {
                if (entity.Database != null)
                {
                    return entity.Database;
                }
                return mainInformation.Database;
            }
        }

        public Mappers GetMappers
        {
            get
            {
                if (entity.Mappers != null)
                {
                    return entity.Mappers;
                }
                return mainInformation.Mappers;
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

        public Entity Entity => entity;


        public ClassBuilder GetClassBuilder(EntityLocation entitylocation)
        {
            if (!string.IsNullOrEmpty(entitylocation.Id) &&_classBuilderDictionary.ContainsKey(entitylocation.Id))
            {
                return _classBuilderDictionary[entitylocation.Id];
            }
            var classBuilder = new ClassBuilder(entity, entitylocation, mainInformation);
            var id = entitylocation.Id ?? "LocationId" + _classBuilderDictionary.Count + 1;
            _classBuilderDictionary.Add(id, classBuilder);
            return classBuilder;
        }

        public ClassBuilder GetClassBuilder(string id)
        {
            if (_classBuilderDictionary.ContainsKey(id))
            {
                return _classBuilderDictionary[id];
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public DatabaseBuilder GetDatabaseBuilder => _database ?? (_database = new DatabaseBuilder(this));
        public MapperBuilder GetMapperBuilder => new MapperBuilder(this);
        public RepositoryBuilder GetRepositoryBuilder => new RepositoryBuilder(this, mainInformation);


    }
}
