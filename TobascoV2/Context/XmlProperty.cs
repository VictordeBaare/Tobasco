using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TobascoV2.Enums;

namespace TobascoV2.Context
{
    public class XmlProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("required")]
        public bool Required { get; set; }

        [XmlElement("DataType")]
        public XmlPropertyType PropertyType { get; set; }

        public string Description { get; set; }        
    }
}
