using System.Xml;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class InterfaceImplementation
    {
        [XmlElement("Name")]
        public ValueElement Name { get; set; }
    }
}