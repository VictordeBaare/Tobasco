using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Tobasco.Model;

namespace Tobasco.Manager
{
    public static class EntityManager
    {
        private static Dictionary<string, string> _nameWithPath;
        private static Dictionary<string, Func<string, EntityHandler>> _entityHandlers;
        private static XmlSerializer _entityserializer = new XmlSerializer(typeof(Entity));

        public static void Initialise(List<NameWithPath> entities)
        {
            _nameWithPath = new Dictionary<string, string>();
            _entityHandlers = new Dictionary<string, Func<string, EntityHandler>>();
            foreach (var entity in entities)
            {
                if (!_entityHandlers.ContainsKey(entity.Name))
                {
                    _entityHandlers.Add(entity.Name, GetHandler);
                    _nameWithPath.Add(entity.Name, entity.Path);
                }
                else
                {
                    throw new ArgumentException($"{entity.Name} already exists.");
                }
            }
        }

        public static Dictionary<string, Func<string, EntityHandler>> EntityHandlers => _entityHandlers;
        
        public static EntityHandler GetEntityOnName(string name)
        {
            if (_entityHandlers.ContainsKey(name))
            {
                return _entityHandlers[name](name);
            }
            throw new ArgumentOutOfRangeException(nameof(name), $"There is no entity with the name: {name}");
        }

        private static EntityHandler GetHandler(string name)
        {
            var filepath = _nameWithPath[name];

            try
            {
                using (var reader = new StreamReader(filepath))
                {
                    var entity = (Entity)_entityserializer.Deserialize(reader);
                    return new EntityHandler(entity);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error with reading xml: {filepath}", ex);
            }
        }
    }
}
