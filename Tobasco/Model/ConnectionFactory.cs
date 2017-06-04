using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class ConnectionFactory
    {
        [XmlAttribute("overridekey")]
        public string Overridekey { get; set; }

        public FileLocation FileLocation { get; set; }

        public FileLocation InterfaceLocation { get; set; }
    }
}
