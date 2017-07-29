using System.Security;

namespace Tobasco.FileBuilder
{
    public class TypeWithName
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public virtual string Build()
        {
            return $"{Type} {Name}";
        }
    }
}