using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class GenerateElement
    {
        [XmlAttribute("generate")]
        public bool Generate { get; set; }
    }
}