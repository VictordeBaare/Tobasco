using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Security
    {
        [XmlAttribute("overridekey")]
        public string Overridekey { get; set; }

        [XmlAttribute("generate")]
        public bool Generate { get; set; }

        public Dac Dac { get; set; }

    }
}