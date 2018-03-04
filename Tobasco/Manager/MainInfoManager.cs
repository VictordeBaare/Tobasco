using System.Collections.Generic;
using System.Linq;
using Tobasco.Model;

namespace Tobasco.Manager
{
    public static class MainInfoManager
    {        
        public static void Initialize(EntityInformation entityInformation)
        {
            EntityInformation = entityInformation;
        }

        public static EntityInformation EntityInformation { get; private set; }

        public static IEnumerable<string> GetBasicNamespaces
        {
            get { return EntityInformation.Namespaces.Select(x => x.Value); }
        }

        public static string GetEnumNamespace { get { return EntityInformation.EnumNamespace.Value; } }
    }
}
