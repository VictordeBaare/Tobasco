using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Manager
{
    public static class EntityManager
    {
        private static Dictionary<string, EntityHandler> EntityHandlers;

        public static void Initialise(Dictionary<string, EntityHandler> entities)
        {
            EntityHandlers = entities;
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
