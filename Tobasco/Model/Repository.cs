using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Repository : FileLocation
    {
        public FileLocation InterfaceLocation { get; set; }

        [XmlAttribute("EntityId")]
        public string EntityId { get; set; }

        [XmlArray("Namespaces")]
        [XmlArrayItem("Namespace", typeof(ValueElement))]
        public List<ValueElement> Namespaces { get; set; }

        public Transaction Transaction { get; set; }

        [XmlAttribute("generate")]
        public bool Generate { get; set; }
    }
}
