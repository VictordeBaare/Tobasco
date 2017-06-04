using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class EntityLocation
    {
        public FileLocation FileLocation { get; set; }

        [XmlAttribute("generaterules")]
        public bool GenerateRules { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("overridekey")]
        public string Overridekey { get; set; }

        public ORMapper ORMapper { get; set; }

        public ValueElement BaseClass { get; set; }      

        [XmlArray("Namespaces")]
        [XmlArrayItem("Namespace", typeof(ValueElement))]
        public List<ValueElement> Namespaces { get; set; }

        [XmlIgnore]
        public string GetBaseClass
        {
            get
            {
                if (BaseClass != null)
                {
                    return ": " + BaseClass.Value;
                }
                if (ORMapper != null)
                {
                    switch (ORMapper.Type)
                    {
                        case Enums.OrmType.Dapper:                        
                            return ": EntityBase";                        
                        default:
                            return string.Empty;
                    }
                }
                return string.Empty;
            }
        }
    }
}
