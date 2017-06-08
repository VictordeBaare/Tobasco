using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Entity : EntityInformation
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("isabstract")]
        public bool IsAbstract { get; set; }

        [XmlArray("Properties")]
        [XmlArrayItem("Property", typeof(Property))]
        public List<Property> Properties { get; set; }

        [XmlAttribute("generateReadonlyGuid")]
        public bool GenerateReadonlyGuid { get; set; }
    }
}
