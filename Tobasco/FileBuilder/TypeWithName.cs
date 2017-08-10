using System.Security;

namespace Tobasco.FileBuilder
{
    public class TypeWithName
    {
        public TypeWithName(string name, string type)
        {
            Type = type;
            Name = name;
        }

        public string Type { get; }

        public string Name { get; }

        public virtual string Build()
        {
            return $"{Type} {Name}";
        }
    }
}