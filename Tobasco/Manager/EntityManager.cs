using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Tobasco.Model;

namespace Tobasco.Manager
{
    public static class EntityManager
    {
        private static Dictionary<string, LoadedEntity> loadedEntitiesDictionary;
        private static XmlSerializer _entityserializer = new XmlSerializer(typeof(Entity));

        public static void Initialise(List<LoadedEntity> entities)
        {
            loadedEntitiesDictionary = new Dictionary<string, LoadedEntity>();
            EntityHandlers = new Dictionary<string, Func<string, EntityHandler>>();
            foreach (var entity in entities)
            {
                if (!EntityHandlers.ContainsKey(entity.Name))
                {
                    EntityHandlers.Add(entity.Name, GetHandler);
                    loadedEntitiesDictionary.Add(entity.Name, entity);
                }
                else
                {
                    throw new ArgumentException($"{entity.Name} already exists.");
                }
            }
        }

        public static Dictionary<string, Func<string, EntityHandler>> EntityHandlers;
        
        public static EntityHandler GetEntityOnName(string name)
        {
            if (EntityHandlers.ContainsKey(name))
            {
                return EntityHandlers[name](name);
            }
            throw new ArgumentOutOfRangeException(nameof(name), $"There is no entity with the name: {name}");
        }

        private static EntityHandler GetHandler(string name)
        {
            var loadedEntity = loadedEntitiesDictionary[name];

            try
            {
                using (var reader = new StreamReader(loadedEntity.Path))
                {
                    var entity = (Entity)_entityserializer.Deserialize(reader);
                    return new EntityHandler(entity, loadedEntity.IsChanged);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error with reading xml: {loadedEntity.Path}", ex);
            }
        }
    }
}
