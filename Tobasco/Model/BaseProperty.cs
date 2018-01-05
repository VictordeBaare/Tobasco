using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class BaseProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("required")]
        public bool Required { get; set; }

        public DataType DataType { get; set; }

        public string Description { get; set; }
    }
}
