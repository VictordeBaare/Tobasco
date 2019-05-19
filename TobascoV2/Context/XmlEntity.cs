using System.Collections.Generic;
using System.Xml.Serialization;
using TobascoV2.Context.Base;

namespace TobascoV2.Context
{
    [XmlRoot("Entity")]
    public class XmlEntity 
    {
        public FileLocation EntityLocation { get; set; }

        public string BaseClass { get; set; }

        public List<string> Namespaces { get; } = new List<string>();         

        public bool IsAbstract { get; set; }

        [XmlArray("Properties")]
        [XmlArrayItem("Property", typeof(XmlProperty))]
        public List<XmlProperty> Properties { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        public DatabaseContext DatabaseContext { get; set; }
    }
}
