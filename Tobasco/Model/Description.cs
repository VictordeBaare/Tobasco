using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Description
    {
        [XmlAttribute("required")]
        public bool Required { get; set; }
    }
}
