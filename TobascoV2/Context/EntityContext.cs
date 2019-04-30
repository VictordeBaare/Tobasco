using System.Collections.Generic;
using TobascoV2.Context.Base;

namespace TobascoV2.Context
{
    public class EntityContext 
    {
        public FileLocation EntityLocation => new FileLocation();

        public string BaseClass { get; set; }

        public List<string> Namespaces { get; } = new List<string>();         

        public bool IsAbstract { get; set; }
    }
}
