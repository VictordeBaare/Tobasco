using System;
using System.Collections.Generic;
using Tobasco.Model;

namespace Tobasco.Manager
{
    public static class EntityManager
    {
        public static Dictionary<string, EntityHandler> EntityHandlers { get; private set; }

        public static void Initialise(List<Entity> entities)
        {
            EntityHandlers = new Dictionary<string, EntityHandler>();
            foreach (var entity in entities)
            {
                if (!EntityHandlers.ContainsKey(entity.Name))
                {
                    EntityHandlers.Add(entity.Name, new EntityHandler(entity));
                }
                else
                {
                    throw new ArgumentException($"{entity.Name} already exists.");
                }
            }
        }

        public static EntityHandler GetEntityOnName(string name)
        {
            if (EntityHandlers.ContainsKey(name))
            {
                return EntityHandlers[name];
            }
            throw new ArgumentOutOfRangeException(nameof(name), $"There is no entity with the name: {name}");
        }
    }
}
