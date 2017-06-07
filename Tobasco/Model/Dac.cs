using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Dac
    {
        public ValueElement BaseClass { get; set; }

        public ORMapper ORMapper { get; set; }

        [XmlArray("Properties")]
        [XmlArrayItem("Property", typeof(Property))]
        public List<Property> Properties { get; set; }

        [XmlAttribute("generate")]
        public bool Generate { get; set; }

        public FileLocation FileLocation { get; set; }

        public Database Database { get; set; }

        public Repository Repository { get; set; }

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

        public string Name(string entityName)
        {
            return $"{entityName}Dac";
        }
    }
}