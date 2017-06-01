using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Tables : FolderWithGenerate
    {
        [XmlElement("Historie")]
        public GenerateElement GenerateHistorie { get; set; }

    }
}