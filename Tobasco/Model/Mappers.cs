using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Mappers
    {
        [XmlAttribute("generate")]
        public bool Generate { get; set; }

        [XmlElement("Mapper")]
        public List<Mapper> Mapper { get; set; }

        [XmlAttribute("overridekey")]
        public string Overridekey { get; set; }
    }
}
