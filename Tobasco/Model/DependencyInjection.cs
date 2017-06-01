using System.Collections.Generic;
using System.Windows.Documents;
using System.Xml.Serialization;
using Tobasco.Enums;

namespace Tobasco.Model
{
    public class DependencyInjection
    {
        [XmlAttribute("type")]
        public DIType Type { get; set; }

        [XmlArray("Modules")]
        [XmlArrayItem("Module", typeof(Module))]
        public List<Module> Modules { get; set; }
    }
}